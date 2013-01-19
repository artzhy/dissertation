using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkOrderSvc" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WorkOrderSvc.svc or WorkOrderSvc.svc.cs at the Solution Explorer and start debugging.
    public class WorkOrderSvc : IWorkOrderSvc {
        
        public BusinessLayer.WorkOrder CreateWorkOrder(AuthToken at, int deviceId, int applicationId, string commPackageJson) {
            new AuthSvc().AuthUser(at, -1, deviceId);

           return  BusinessLayer.WorkOrder.CreateWorkOrder(deviceId, applicationId, commPackageJson);
        }

        public void CancelWorkOrder(AuthToken at, int workOrderId) {
            new AuthSvc().AuthUser(at);
            BusinessLayer.WorkOrder wo = BusinessLayer.WorkOrder.Populate(workOrderId);

            if (wo.UserDevice.User.Username != at.Username)
                throw new Exception("Cannot delete Work Order which you do not own");

            wo.Delete();
        }
    }
}
