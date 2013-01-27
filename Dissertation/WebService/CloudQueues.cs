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
        public const String UpdatedWorkOrderQueue = "updatedworkorders";

        public static String ConnectionString {
            get {
                return ConfigurationSettings.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            }
        }


        private static QueueClient _NewWorkOrders;
        private static QueueClient _UpdatedWorkOrders;

        private CloudQueues() {

        }

        public static QueueClient NewWorkOrderQueueClient {
            get {
                if (_NewWorkOrders == null) 
                    _NewWorkOrders = QueueClient.CreateFromConnectionString(ConnectionString, NewWorkOrderQueue);
                
                return _NewWorkOrders;
            }
        }

        public static QueueClient UpdatedWorkOrderQueueClient {
            get {
                if (_UpdatedWorkOrders == null)
                    _UpdatedWorkOrders = QueueClient.CreateFromConnectionString(ConnectionString, UpdatedWorkOrderQueue);
                return _UpdatedWorkOrders;
            }
        }




    }
}