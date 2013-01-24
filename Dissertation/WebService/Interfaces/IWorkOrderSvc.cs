﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebService {
    [ServiceContract]
    public interface IWorkOrderSvc {
        [OperationContract]
        [WebGet(UriTemplate = "CreateWorkOrder", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        BusinessLayer.WorkOrder CreateWorkOrder(String at, int deviceId, int applicationId, String commPackageJson);

        [OperationContract]
        [WebGet(UriTemplate = "CancelWorkOrder", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void CancelWorkOrder(String at, int workOrderId);

        [OperationContract]
        [WebGet(UriTemplate = "GetWorkOrder", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        WorkOrderTrimmed GetWorkOrder(String at, int deviceId, int workOrderId);

        //TODO: Get status of work order

    }
}
