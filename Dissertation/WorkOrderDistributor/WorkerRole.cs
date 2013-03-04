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
        private DateTime PackageSweepLastDone;
        private Boolean PackageSweepOccuring = false;

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient NewWorkOrders;
        QueueClient UpdatedWorkOrders;
        QueueClient CommunicationPackages;
        List<Task> TaskList;

        bool IsStopped;


        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder,
            Cancel
        }


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

                    if (PackageSweepLastDone <= DateTime.Now.AddSeconds(-45) && !PackageSweepOccuring) {
                        DoCommPackageSweep();
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

        private void DoCommPackageSweep() {
            PackageSweepOccuring = true;

            List<CommunicationPackage> unAckedPackages = Scheduler.GetUnacknowledgedPackages(180);
            foreach (CommunicationPackage pkg in unAckedPackages) {
                CommunicationPackage oPkg = CommunicationPackage.Populate(pkg.CommunicationId);
                CommunicationPackages.Send(new BrokeredMessage(oPkg.Serialize()));
            }

            
            //foreach (CommunicationPackage pkg in unAckedPackages) {



            //    // If new WO/update request, re-schedule WO
            //    if ((UpdateType)pkg.CommunicationType == UpdateType.NewWorkOrder || (UpdateType)pkg.CommunicationType == UpdateType.UpdateRequest) {

            //        try {
            //            // Set old comm package status to cancelled
            //            CommunicationPackage oPkg = CommunicationPackage.Populate(pkg.CommunicationId);

            //            if (oPkg.SendAttempts < 3) {
            //                CommunicationPackages.Send(new BrokeredMessage(oPkg.Serialize()));
            //            } else {
            //                try {
            //                    WorkOrder wo = WorkOrder.Populate(pkg.WorkOrderId);
            //                    wo.SlaveWorkerId = GetAvailableDevice(wo.ApplicationId);
            //                    wo.Save();

            //                    oPkg.Status = "CANCELLED";
            //                    oPkg.Save();

            //                    // Create new comm package and send
            //                    CommunicationPackage cp = CommunicationPackage.CreateCommunication((int)wo.SlaveWorkerId, CommunicationPackage.UpdateType.NewWorkOrder, wo.WorkOrderId);

            //                    CommunicationPackages.Send(new BrokeredMessage(cp.Serialize()));

            //                } catch {
            //                    // Error likely to be sent here due to being unable to find a slave worker.
            //                    Debug.WriteLine("Unable to find suitable slave.  For WO: " + oPkg.WorkOrderId);
            //                }
            //            }

            //        } catch (Exception) {
            //            // Dont do anything - most likely cause of this is due to no device available to submit to.
            //        }

            //    // If result/cancel, re-send package
            //    } else if ((UpdateType)pkg.CommunicationType == UpdateType.Result || (UpdateType)pkg.CommunicationType == UpdateType.Cancel) {

            //        if (pkg.SendAttempts <= 3) {
            //            // Retry, otherwise do nothing
            //            CommunicationPackages.Send(new BrokeredMessage(pkg.Serialize()));
            //        }    
            //    }

            //    try {
            //        // Remove device from active list
            //        // We can do this as devices will notify that they are active if this is true every X seconds.
            //  //      ActiveDevice.Populate(pkg.TargetDeviceId).Delete();

            //    } catch {
            //        //Dont do anything - may already be deleted.
            //    }
            //}

            PackageSweepOccuring = false;
            PackageSweepLastDone = DateTime.Now;
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

                try {
                    wo.SlaveWorkerId = GetAvailableDevice(wo.ApplicationId);
                    wo.Save();

                    CommunicationPackage cp = CommunicationPackage.CreateCommunication((int)wo.SlaveWorkerId, CommunicationPackage.UpdateType.NewWorkOrder, wo.WorkOrderId);


                    CommunicationPackages.Send(new BrokeredMessage(cp.Serialize()));

                   

                } catch {
                   // No device available requeue the work order.

                    NewWorkOrders.Send(new BrokeredMessage(wo.SlaveWorkerId));

                }

                newWorkOrderMessage.Complete();
            }
        }

        private int GetAvailableDevice(int appId) {
            return BusinessLayer.Scheduler.GetAvailableSlave(appId).DeviceId;
        }

        private void ProcessUpdatedWorkOrders() {
            try {
                BrokeredMessage updatedWorkOrderMessage = null;
                updatedWorkOrderMessage = UpdatedWorkOrders.Receive();

                if (updatedWorkOrderMessage != null) {
                    // Process the message
                    SharedClasses.WorkOrderUpdate wou = updatedWorkOrderMessage.GetBody<SharedClasses.WorkOrderUpdate>();
                    WorkOrder wo = WorkOrder.Populate(wou.WorkOrderId);


                    Trace.WriteLine("Processing Update", wou.WorkOrderId.ToString());

                    switch (wou.WorkOrderUpdateType) {
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
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        private void ProcessCommunications() {
            try {
                BrokeredMessage commMessage = null;
                commMessage = CommunicationPackages.Receive();

                if (commMessage != null) {

                    CommunicationPackage cp = Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationPackage>(commMessage.GetBody<String>());
                    CommunicationPackage oCP = CommunicationPackage.Populate(cp.CommunicationId);
                    oCP.SendAttempts++;
                    oCP.Save();

                    UserDevice targetUD = UserDevice.Populate(cp.TargetDeviceId);
                    // If the last send was less than 3 mins ago, send a GCM message
                    if (targetUD.ActiveDevice.LastActiveSend < DateTime.Now.AddMinutes(-3)) {
                        Pusher.SendNotification(targetUD.GCMCode, cp.Serialize());
                    }

                    commMessage.Complete();

                    Debug.WriteLine("Communication ID: " + cp.CommunicationId + " sent.");
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
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
            PackageSweepLastDone = DateTime.Now.AddSeconds(-60);

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
