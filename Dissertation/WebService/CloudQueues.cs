using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Configuration;

namespace WebService {
    public class CloudQueues {
        public const String NewWorkOrderQueue = "newworkorders";
        public const String CompletedWorkOrderQueue = "completedworkorders";
        public const String CancelledWorkOrderQueue = "cancelledworkorders";

        public static String ConnectionString {
            get {
                return ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            }
        }


        private static QueueClient _NewWorkOrders;
        private static QueueClient _CompletedWorkOrders;
        private static QueueClient _CancelledWorkOrders;

        private CloudQueues() {

        }

        public static QueueClient NewWorkOrderQueueClient {
            get {
                if (_NewWorkOrders == null) 
                    _NewWorkOrders = QueueClient.CreateFromConnectionString(ConnectionString, NewWorkOrderQueue);
                
                return _NewWorkOrders;
            }
        }

        public static QueueClient CompletedWorkOrderQueueClient {
            get {
                if (_CompletedWorkOrders == null)
                    _CompletedWorkOrders = QueueClient.CreateFromConnectionString(ConnectionString, CompletedWorkOrderQueue);
                return _CompletedWorkOrders;
            }
        }


        public static QueueClient CancelledWorkOrderQueueClient {
            get {
                if (_CompletedWorkOrders == null)
                    _CompletedWorkOrders = QueueClient.CreateFromConnectionString(ConnectionString, CompletedWorkOrderQueue);
                return _CompletedWorkOrders;
            }
        }




    }
}