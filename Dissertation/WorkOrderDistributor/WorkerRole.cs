using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

using BusinessLayer;
using System.Threading.Tasks;

using SharedClasses;

namespace WorkOrderDistributor {
    public class WorkerRole : RoleEntryPoint {
        // The name of your queue
        const string NewWorkOrdersQName = "newworkorders";
        const string UpdatedWorkOrersQName = "updatedworkorders";
        const string CommunicationPackagesQName = "communicationpackages";
        private PushCommunicator.Pusher Pusher;


        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient NewWorkOrders;
        QueueClient UpdatedWorkOrders;
        QueueClient CommunicationPackages;
        List<Task> TaskList;


        bool IsStopped;

        public override void Run() {
            while (!IsStopped) {
                try {
                    // Remove completed tasks.
                    RemoveCompletedTasks();

                    if (TaskList.Count < 10) {
                        var taskProcessNew = Task.Factory.StartNew(() => ProcessNewWorkOrders());
                        TaskList.Add(taskProcessNew);
                        var taskProcessUpdates = Task.Factory.StartNew(() => ProcessUpdatedWorkOrders());
                        TaskList.Add(taskProcessUpdates);
                        var taskProcessCommunicate = Task.Factory.StartNew(() => ProcessCommunications());
                        TaskList.Add(taskProcessCommunicate);
                    } else {
                        Task.WaitAny(TaskList.ToArray());
                    }
                                     
                } catch (MessagingException e) {
                    if (!e.IsTransient) {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    Thread.Sleep(10000);
                } catch (OperationCanceledException e) {
                    if (!IsStopped) {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }

        private void RemoveCompletedTasks() {
            List<Task> toRemove = new List<Task>();

            foreach (Task t in TaskList) {
                if (t.IsCompleted || t.IsCanceled)
                    toRemove.Add(t);
                else if (t.IsFaulted) {
                    Trace.WriteLine(t.Exception);
                    toRemove.Add(t);
                }
            }

            foreach (Task t in toRemove) {
                TaskList.Remove(t);
            }
        }

        private void ProcessNewWorkOrders() {
            BrokeredMessage newWorkOrderMessage = null;
            newWorkOrderMessage = NewWorkOrders.Receive();

            if (newWorkOrderMessage != null) {
                // Process the message
                Trace.WriteLine("Processing", newWorkOrderMessage.SequenceNumber.ToString());

                WorkOrder wo = WorkOrder.Populate(newWorkOrderMessage.GetBody<int>());

                wo.SlaveWorkerId = GetAvailableDevice(wo.WorkOrderId);
                wo.Save();

                CommunicationPackage cp = CommunicationPackage.CreateCommunication((int)wo.SlaveWorkerId, CommunicationPackage.UpdateType.NewWorkOrder, wo.WorkOrderId);

                CommunicationPackages.Send(new BrokeredMessage(cp.Serialize()));

                newWorkOrderMessage.Complete();
            }
        }

        private int GetAvailableDevice(int workOrderId) {
            return BusinessLayer.Scheduler.GetAvailableSlave().DeviceId;
        }

        private void ProcessUpdatedWorkOrders() {
            BrokeredMessage updatedWorkOrderMessage = null;
            updatedWorkOrderMessage = UpdatedWorkOrders.Receive();

            if (updatedWorkOrderMessage != null) {
                // Process the message
                SharedClasses.WorkOrderUpdate wou = updatedWorkOrderMessage.GetBody<SharedClasses.WorkOrderUpdate>();
                WorkOrder wo = WorkOrder.Populate(wou.WorkOrderId);
                

                Trace.WriteLine("Processing Update", wou.WorkOrderId.ToString());

                switch(wou.WorkOrderUpdateType) {
                    case SharedClasses.WorkOrderUpdate.UpdateType.Acknowledge:
                        // Update database with last spoke time and message
                        wo.SlaveWorkOrderLastCommunication = DateTime.Now;
                        wo.WorkOrderStatus = "SLAVE_ACKNOWLEDGED";
                        wo.Save();
                       
                        break;
                    case SharedClasses.WorkOrderUpdate.UpdateType.Cancel:
                        // TODO: Implement cancel procedure
                        // Check if device is handlign WO

                        // If device is not handlign work order, cancel it.

                        // If device is handlin work order, notify device to cancel, and then mark cancelled in DB.
                        break;
                    case SharedClasses.WorkOrderUpdate.UpdateType.SubmitResult:
                        // Update database with result.
                        wo.SlaveWorkOrderLastCommunication = DateTime.Now;
                        wo.WorkOrderResultJson = wou.ResultJson;
                        wo.WorkOrderStatus = "RESULT_RECEIVED";
                        wo.StartComputationTime = wou.ComputationStartTime;
                        wo.EndComputationTime = wou.ComputationEndTime;
                        
                        wo.Save();

                        // Send result to requesting device

                        CommunicationPackage cp = CommunicationPackage.CreateCommunication(wo.DeviceId, CommunicationPackage.UpdateType.Result, wo.WorkOrderId);

                        CommunicationPackages.Send(new BrokeredMessage(cp.Serialize()));
              

                        //TODO: Send new WO to the slave device
                        break;

                    case SharedClasses.WorkOrderUpdate.UpdateType.MarkBeingComputed:
                        // Update database
                        wo.SlaveWorkOrderLastCommunication = DateTime.Now;
                        wo.WorkOrderStatus = "BEING_COMPUTED";
                        wo.Save();

                        break;
                }

                updatedWorkOrderMessage.Complete();
            }
        }

        private void ProcessCommunications() {
            BrokeredMessage commMessage = null;
            commMessage = CommunicationPackages.Receive();

            if (commMessage != null) {

                CommunicationPackage cp = Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationPackage>(commMessage.GetBody<String>());

                UserDevice targetUD = UserDevice.Populate(cp.TargetDeviceId);

                Pusher.SendNotification(targetUD.GCMCode, cp.Serialize());

                commMessage.Complete();

                Debug.WriteLine("Communication ID: " + cp.CommunicationId + " sent.");
            }
        }

        public override bool OnStart() {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(NewWorkOrdersQName)) {
                namespaceManager.CreateQueue(NewWorkOrdersQName);
            }

            if (!namespaceManager.QueueExists(UpdatedWorkOrersQName)) {
                namespaceManager.CreateQueue(UpdatedWorkOrersQName);
            }

            if (!namespaceManager.QueueExists(CommunicationPackagesQName)) {
                namespaceManager.CreateQueue(CommunicationPackagesQName);
            }

         
            // Initialize the connection to Service Bus Queue
            NewWorkOrders = QueueClient.CreateFromConnectionString(connectionString, NewWorkOrdersQName);
            UpdatedWorkOrders = QueueClient.CreateFromConnectionString(connectionString, UpdatedWorkOrersQName);
            CommunicationPackages = QueueClient.CreateFromConnectionString(connectionString, CommunicationPackagesQName);
            Pusher = new PushCommunicator.Pusher();


            TaskList = new List<Task>();

            IsStopped = false;
            return base.OnStart();
        }

        public override void OnStop() {
            // Close the connection to Service Bus Queue
            IsStopped = true;
            NewWorkOrders.Close();
            UpdatedWorkOrders.Close();
            CommunicationPackages.Close();
            Task.WaitAll(TaskList.ToArray());

            base.OnStop();
        }
    }
}
