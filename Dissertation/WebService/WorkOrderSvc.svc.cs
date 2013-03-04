using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;

using SharedClasses;


namespace WebService {
    public class WorkOrderSvc : IWorkOrderSvc {
        
        public BusinessLayer.WorkOrder CreateWorkOrder(String at, int deviceId, int applicationId, String paramListJson, String backgroundProcessFunction, int localDeviceWORef) {
            new AuthSvc().AuthUser(at, -1, deviceId);

            BusinessLayer.WorkApplication wa = BusinessLayer.WorkApplication.Populate(applicationId);
           CommPackage cp = new CommPackage();
           cp.BackgroundProcessClass = "";
           cp.BackgroundProcessFunction = backgroundProcessFunction;
           cp.ParameterList = CommPackage.DeserializeParamJson(paramListJson);
           cp.DeviceUIRef = wa.ApplicationUIResultIntent;
           cp.DeviceLocalRequestId = localDeviceWORef;

            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.CreateWorkOrder(deviceId, applicationId, localDeviceWORef);

            cp.ComputationRequestId = wo.WorkOrderId;
            wo.CommPackageJson = cp.SerializeJson();
            wo.Save();

           CloudQueues.NewWorkOrderQueueClient.Send(new BrokeredMessage(wo.WorkOrderId));

           return wo;
        }

        public WorkOrderTrimmed GetWorkOrder(String at, int deviceId, int workOrderId) {
            new AuthSvc().AuthUser(at, -1, deviceId);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);
            BusinessLayer.WorkApplication wa = BusinessLayer.WorkApplication.Populate(wo.ApplicationId);
           

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
            wt.ComputeAppIntent = wa.ApplicationWorkIntent;
            wt.ApplicationUIResultIntent = wa.ApplicationUIResultIntent;
            wt.DeviceLocalRequestId = wo.LocalDeviceId;
            
            return wt;
        }

        public void CancelWorkOrder(String at, int workOrderId) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);

            if (wo.DeviceId != oAt.DeviceId)
                throw new Exception("Cannot delete Work Order which you do not own");

            CloudQueues.UpdatedWorkOrderQueueClient.Send(new BrokeredMessage(new SharedClasses.WorkOrderUpdate(workOrderId, SharedClasses.WorkOrderUpdate.UpdateType.Cancel, oAt.DeviceId, null, null)));
            
        }

        public void AcknowledgeWorkOrder(String at, int workOrderId) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);


            //TODO: Fix this auth issue
            //if (wo.SlaveWorkerId != oAt.DeviceId)
            //    throw new Exception("Cannot modify Work Order which you are not meant to be working on.");

            CloudQueues.UpdatedWorkOrderQueueClient.Send(new BrokeredMessage(new SharedClasses.WorkOrderUpdate(workOrderId, SharedClasses.WorkOrderUpdate.UpdateType.Acknowledge, oAt.DeviceId, null, null)));

        }

        public void MarkWorkOrderInComputation(string at, int workOrderId) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);

            if (wo.SlaveWorkerId != oAt.DeviceId)
                throw new Exception("Cannot modify Work Order which you are not meant to be working on.");

            CloudQueues.UpdatedWorkOrderQueueClient.Send(new BrokeredMessage(new SharedClasses.WorkOrderUpdate(workOrderId, SharedClasses.WorkOrderUpdate.UpdateType.MarkBeingComputed, oAt.DeviceId, null, null)));
        }

        public void SubmitWorkOrderResult(string at, int workOrderId, String resultJson, DateTime compuatationStartTime, DateTime computationEndTime) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);

            //if (wo.SlaveWorkerId != oAt.DeviceId)
            //    throw new Exception("Cannot modify Work Order which you are not meant to be working on.");

            CloudQueues.UpdatedWorkOrderQueueClient.Send(new BrokeredMessage(new SharedClasses.WorkOrderUpdate(workOrderId, SharedClasses.WorkOrderUpdate.UpdateType.SubmitResult, oAt.DeviceId, compuatationStartTime, computationEndTime, resultJson)));

        }

        public void AcknowledgeCommunication(String at, int communicationId, DateTime recieveTime) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at);
            BusinessLayer.CommunicationPackage cp = BusinessLayer.CommunicationPackage.Populate(communicationId);

            cp.DateAcknowledged = recieveTime;
            cp.Response = "ACK";

            cp.Save();

        }

        public List<BusinessLayer.CommunicationPackage> GetOutstandingCommunications(String at, int deviceId) {
            BusinessLayer.AuthenticationToken oAt = new AuthSvc().AuthUser(at,-1,deviceId);

            BusinessLayer.ActiveDevice ad = BusinessLayer.ActiveDevice.Populate(deviceId);
            ad.LastFetch = DateTime.Now;
            ad.Save();

            return BusinessLayer.CommunicationPackage.GetTargetDeviceCommunications(deviceId);
        }

    }
}
