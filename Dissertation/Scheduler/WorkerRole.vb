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

namespace Scheduler {
    public class WorkerRole : RoleEntryPoint {
        // The name of your queue
        const string NewWorkOrdersQName = "newworkorders";
        const string CompletedWorkOrdersQName = "completedworkorders";

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient NewWorkOrders;
        QueueClient CompletedWorkOrders;

        bool IsStopped;

        public override void Run() {
            while (!IsStopped) {
                try {
                    // Receive the message
                    BrokeredMessage receivedMessage = null;
                    receivedMessage = NewWorkOrders.Receive();


                    if (receivedMessage != null) {
                        // Process the message
                        Trace.WriteLine("Processing", receivedMessage.SequenceNumber.ToString());
                        receivedMessage.Complete();
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

        public override bool OnStart() {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(NewWorkOrdersQName)) {
                namespaceManager.CreateQueue(NewWorkOrdersQName);
            }

            if (!namespaceManager.QueueExists(CompletedWorkOrdersQName)) {
                namespaceManager.CreateQueue(CompletedWorkOrdersQName);
            }

            // Initialize the connection to Service Bus Queue
            NewWorkOrders = QueueClient.CreateFromConnectionString(connectionString, NewWorkOrdersQName);
            CompletedWorkOrders = QueueClient.CreateFromConnectionString(connectionString, CompletedWorkOrdersQName);
            IsStopped = false;
            return base.OnStart();
        }

        public override void OnStop() {
            // Close the connection to Service Bus Queue
            IsStopped = true;
            NewWorkOrders.Close();
            CompletedWorkOrders.Close();

            base.OnStop();
        }
    }
}
