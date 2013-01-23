using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWorkOrderSvc" in both code and config file together.
    [ServiceContract]
    public interface IWorkOrderSvc {
        [OperationContract]
        BusinessLayer.WorkOrder CreateWorkOrder(String at, int deviceId, int applicationId, String commPackageJson);

        [OperationContract]
        void CancelWorkOrder(String at, int workOrderId);

        //TODO: Get status of work order

    }
}
