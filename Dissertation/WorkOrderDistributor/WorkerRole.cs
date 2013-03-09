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
        private Boolean WOSweepOccuring = false;
        private Boolean ProcessingNewWorkOrders = false;
        private Boolean ProcessingUpdatedWorkOrders = false;


        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient NewWorkOrders;
        QueueClient UpdatedWorkOrders;
        QueueClient CommunicationPackages;
        List<Task> TaskList;
        TimeSpan ThreadSleepTime = new TimeSpan(0, 0, 15);

        bool IsStopped;

        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder,
            Cancel,
            FetchRequest
        }

        public override void Run() {
            while (!IsStopped) {
                try {
                    // Remove completed tasks.
                    // RemoveCompletedTasks();

                    if (!PackageSweepOccuring)
                        Task.Factory.StartNew(() => DoCommPackageSweep());
                    if (!WOSweepOccuring)
                        Task.Factory.StartNew(() => DoWOSweep());
                    if (!ProcessingNewWorkOrders)
                        Task.Factory.StartNew(() => ProcessNewWorkOrders());
                    if (!ProcessingUpdatedWorkOrders)
                        Task.Factory.StartNew(() => ProcessUpdatedWorkOrders());
                    //if (!PackageSweepOccuring) {
                    //    DoCommPackageSweep();
                    //}



                    //if (TaskList.Count < 10) {
                    //    //   var taskProcessNew = Task.Factory.StartNew(() => ProcessNewWorkOrders());

                    //    var taskProcessUpdates = Task.Factory.StartNew(() => ProcessUpdatedWorkOrders());

                    //    //var taskProcessCommunicate = Task.Factory.StartNew(() => ProcessCommunications());
                    //    //TaskList.Add(taskProcessCommunicate);
                    //}

                    //} else {
                    //    Task.WaitAny(TaskList.ToArray());
                    //}


                } catch (MessagingException e) {
                    if (!e.IsTransient) {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    //  Thread.Sleep(10000);
                } catch (OperationCanceledException e) {
                    if (!IsStopped) {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                }
                Thread.Sleep(ThreadSleepTime);
            }
        }

        private void DoCommPackageSweep() {
            if (!PackageSweepOccuring) {
                PackageSweepOccuring = true;

                List<CommunicationPackage> unAckedPackages = Scheduler.GetUnacknowledgedPackages(20);


                foreach (CommunicationPackage cp in unAckedPackages) {
                    ProcessCommunication(cp);
                }

                Thread.Sleep(new TimeSpan(0, 0, 15));
                PackageSweepOccuring = false;
            }

        }

        private void DoWOSweep() {
            // while (!IsStopped) {
            if (!WOSweepOccuring) {
                WOSweepOccuring = true;

                List<WorkOrder> WosWaitingLongerThanAverage = WorkOrder.GetWorkOrdersRequiringReassignment();

                // Re-arrange WOs

                foreach (WorkOrder wo in WosWaitingLongerThanAverage) {
                    // Remove device from active ids
                    try {
                        ActiveDevice.Populate(wo.SlaveWorkerId.Value).Delete();
                    } catch {

                    }
                    using (WorkOrder oWO = WorkOrder.Populate(wo.WorkOrderId)) {
                        oWO.WorkOrderStatus = "BEING_REASSIGNED";
                        oWO.Save();
                    }
                    // Rearrange WO
                    NewWorkOrders.Send(new BrokeredMessage(wo.WorkOrderId));
                }

                WOSweepOccuring = false;
            }

            //       Thread.Sleep(ThreadSleepTime);
            //  }
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
            ProcessingNewWorkOrders = true;
            BrokeredMessage newWorkOrderMessage = null;
            newWorkOrderMessage = NewWorkOrders.Receive();
            while (newWorkOrderMessage != null) {

                // Process the message
                Trace.WriteLine("Processing", newWorkOrderMessage.SequenceNumber.ToString());

                using (WorkOrder wo = WorkOrder.Populate(newWorkOrderMessage.GetBody<int>())) {

                    try {
                        wo.WorkOrderStatus = "";

                        wo.SlaveWorkerId = BusinessLayer.Scheduler.GetAvailableSlave(wo.ApplicationId).DeviceId;
                        wo.SlaveWorkerSubmit = DateTime.Now;
                        wo.Save();


                       using(CommunicationPackage cp = CommunicationPackage.CreateCommunication((int)wo.SlaveWorkerId, CommunicationPackage.UpdateType.NewWorkOrder, wo.WorkOrderId)) {
                        ProcessCommunication(cp);
                       }

                    } catch {
                        // No device available requeue the work order.
                        NewWorkOrders.Send(new BrokeredMessage(wo.WorkOrderId));

                    }
                }

                newWorkOrderMessage.Complete();
                newWorkOrderMessage = NewWorkOrders.Receive();
            }

            ProcessingNewWorkOrders = false;
        }

        private void ProcessUpdatedWorkOrders() {
            ProcessingUpdatedWorkOrders = true;
            BrokeredMessage updatedWorkOrderMessage = null;
            updatedWorkOrderMessage = UpdatedWorkOrders.Receive();


            try {
                while (updatedWorkOrderMessage != null) {
                    // Process the message
                    SharedClasses.WorkOrderUpdate wou = updatedWorkOrderMessage.GetBody<SharedClasses.WorkOrderUpdate>();

                    using (WorkOrder wo = WorkOrder.Populate(wou.WorkOrderId)) {
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
                                if (wo.WorkOrderStatus == "SLAVE_ACKNOWLEDGED") {
                                    // Send message to cancel
                                    CommunicationPackage.CreateCommunication(wo.SlaveWorkerId.Value, CommunicationPackage.UpdateType.Cancel, wo.WorkOrderId);
                                }

                                if (wo.CommunicationPackages.Count() > 0) {
                                    // Have to notify device to cancel
                                    foreach (CommunicationPackage cp in wo.CommunicationPackages.Where(x=>x.CommunicationType != (int)CommunicationPackage.UpdateType.Cancel)) {
                                        if (cp.Response == null && cp.Status == null) {
                                            // Cancel
                                            using (CommunicationPackage oCp = CommunicationPackage.Populate(cp.CommunicationId)) {
                                                oCp.Status = "USER_CANCELLED";
                                                oCp.Save();
                                            }

                                        }
                                    }

                                    wo.WorkOrderStatus = "USER_CANCELLED";
                                } else {
                                    wo.WorkOrderStatus = "CANCELLED";
                                }

                                wo.Save();
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

            CommunicationPackage.CreateCommunication(wo.DeviceId, CommunicationPackage.UpdateType.Result, wo.WorkOrderId);

                                //   CommunicationPackages.Send(new BrokeredMessage(cp.Serialize()));


                                break;

                            case SharedClasses.WorkOrderUpdate.UpdateType.MarkBeingComputed:
                                // Update database
                                wo.SlaveWorkOrderLastCommunication = DateTime.Now;
                                wo.WorkOrderStatus = "BEING_COMPUTED";
                                wo.Save();

                                break;
                        }
                    }

                    updatedWorkOrderMessage.Complete();
                    updatedWorkOrderMessage = UpdatedWorkOrders.Receive();
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
            ProcessingUpdatedWorkOrders = false;
        }

        private void ProcessCommunication(CommunicationPackage cp) {
            try {

                //  CommunicationPackage cp = Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationPackage>(cpSerialized);
                UserDevice ud = UserDevice.Populate(cp.TargetDeviceId);

                ActiveDevice ad;
                // Have to try and catch the exception, since might not exist in the current context
                try {
                    ad = ActiveDevice.Populate(ud.DeviceId);
                } catch (Exception) {
                    ad = null;
                }

                CommunicationPackage fetchRequest = CommunicationPackage.CreateFetchRequest(cp.TargetDeviceId);

                // If last fetch from device was less than 3 mins ago, don't send GCM 
                if (ad != null && ad.LastFetch < DateTime.Now.AddMinutes(-1)) {
                    // Send GCM
                    if (fetchRequest.SendAttempts < 3 && fetchRequest.SubmitDate < DateTime.Now.AddSeconds(-30)) {
                        Pusher.SendNotification(ud.GCMCode, fetchRequest.Serialize());
                        fetchRequest.SubmitDate = DateTime.Now;
                        fetchRequest.SendAttempts++;
                        fetchRequest.Save();


                    } else if (fetchRequest.SendAttempts >= 3 && fetchRequest.SubmitDate < DateTime.Now.AddSeconds(-40)) {
                        // Device might not be active, delete it from the active device list.
                        try {
                            ad.Delete();
                        } catch {

                        }

                        // If not results, re-arrange WO
                        List<int?> workOrdersToBeRearranged = ud.CommunicationPackages.Where(x => x.Status == null && x.Response == null && x.CommunicationType != (int)CommunicationPackage.UpdateType.Result).Select(x => x.WorkOrderId).ToList();

                        // Delete current Comm Packages to the device if they are requests for work.
                        ud.DeleteOutstandingSlaveCommPackages();

                        // Rearange work orders
                        foreach (int? woId in workOrdersToBeRearranged) {
                            if (woId.HasValue)
                                NewWorkOrders.Send(new BrokeredMessage(woId.Value));
                        }

                    }
                }

                // handle if less than 10 ... etc..

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
            WOSweepOccuring = false;
            PackageSweepOccuring = false;

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
