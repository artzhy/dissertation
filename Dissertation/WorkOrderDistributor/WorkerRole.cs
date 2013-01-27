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

namespace WorkOrderDistributor {
    public class WorkerRole : RoleEntryPoint {
        // The name of your queue
        const string NewWorkOrdersQName = "newworkorders";
        const string UpdatedWorkOrersQName = "updatedworkorders";

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient NewWorkOrders;
        QueueClient UpdatedWorkOrders;

        bool IsStopped;

        public override void Run() {
            while (!IsStopped) {
                try {
                    ProcessNewWorkOrders();

                    ProcessUpdatedWorkOrders();
                    
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

        private void ProcessNewWorkOrders() {
            BrokeredMessage newWorkOrderMessage = null;
            newWorkOrderMessage = NewWorkOrders.Receive();

            if (newWorkOrderMessage != null) {
                // Process the message
                Trace.WriteLine("Processing", newWorkOrderMessage.SequenceNumber.ToString());
                newWorkOrderMessage.Complete();
            }
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
                        // TODO: Impelment submit procedure
                        // Update database with result.
                        wo.SlaveWorkOrderLastCommunication = DateTime.Now;
                        wo.WorkOrderResultJson = wou.ResultJson;
                        wo.WorkOrderStatus = "RESULT_RECEIVED";
                        wo.Save();

                        break;
                }

                updatedWorkOrderMessage.Complete();
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

         
            // Initialize the connection to Service Bus Queue
            NewWorkOrders = QueueClient.CreateFromConnectionString(connectionString, NewWorkOrdersQName);
            UpdatedWorkOrders = QueueClient.CreateFromConnectionString(connectionString, UpdatedWorkOrersQName);

            IsStopped = false;
            return base.OnStart();
        }

        public override void OnStop() {
            // Close the connection to Service Bus Queue
            IsStopped = true;
            NewWorkOrders.Close();
            UpdatedWorkOrders.Close();

            base.OnStop();
        }
    }
}
