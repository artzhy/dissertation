using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;


namespace WebService {
    public class WorkOrderSvc : IWorkOrderSvc {
        
        public BusinessLayer.WorkOrder CreateWorkOrder(String at, int deviceId, int applicationId, string commPackageJson) {
            new AuthSvc().AuthUser(at, -1, deviceId);

            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.CreateWorkOrder(deviceId, applicationId, commPackageJson);

           CloudQueues.NewWorkOrderQueueClient.Send(new BrokeredMessage(wo.WorkOrderId));

           return wo;
        }

        public WorkOrderTrimmed GetWorkOrder(String at, int deviceId, int workOrderId) {
            new AuthSvc().AuthUser(at, -1, deviceId);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);

            WorkOrderTrimmed wt = new WorkOrderTrimmed();
            wt.ApplicationId = wo.ApplicationId;
            wt.CommPackageJson = wo.CommPackageJson;
            wt.DeviceId = wo.DeviceId;
            wt.ReceiveTime = wo.ReceiveTime;
            wt.SlaveWorkerId = wo.SlaveWorkerId;
            wt.SlaveWorkerSubmit = wo.SlaveWorkerSubmit;
            wt.SlaveWorkOrderLastCommunication = wo.SlaveWorkOrderLastCommunication;
            wt.WorkOrderId = wo.WorkOrderId;
            wt.WorkOrderResultJson = wo.WorkOrderResultJson;
            wt.WorkOrderStatus = wo.WorkOrderStatus;
      
            return wt;
        }

        public void CancelWorkOrder(String at, int workOrderId) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);

            if (wo.UserDevice.User.Username != oAt.Username)
                throw new Exception("Cannot delete Work Order which you do not own");

            CloudQueues.CancelledWorkOrderQueueClient.Send(new BrokeredMessage(wo.WorkOrderId));
            //wo.Delete();
        }
    }
}
