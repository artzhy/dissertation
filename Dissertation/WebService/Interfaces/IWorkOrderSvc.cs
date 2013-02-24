using System;
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
        BusinessLayer.WorkOrder CreateWorkOrder(String at, int deviceId, int applicationId, List<SharedClasses.CommPackage.ParamListItem> paramList, String backgroundProcessFunction);

        [OperationContract]
        [WebGet(UriTemplate = "CancelWorkOrder", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void CancelWorkOrder(String at, int workOrderId);

        [OperationContract]
        [WebGet(UriTemplate = "GetWorkOrder", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        WorkOrderTrimmed GetWorkOrder(String at, int deviceId, int workOrderId);

        [OperationContract]
        [WebGet(UriTemplate = "AcknowledgeWorkOrder", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void AcknowledgeWorkOrder(String at, int workOrderId);

        [OperationContract]
        [WebGet(UriTemplate = "SubmitWorkOrderResult", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void SubmitWorkOrderResult(string at, int workOrderId, String resultJson, DateTime compuatationStartTime, DateTime computationEndTime);

        [OperationContract]
        [WebGet(UriTemplate = "MarkWorkOrderInComputation", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void MarkWorkOrderInComputation(string at, int workOrderId);

        [OperationContract]
        [WebGet(UriTemplate = "AcknowledgeCommunication", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void AcknowledgeCommunication(String at, int communicationId, DateTime recieveTime);


       //TODO: Get status of work order

    }
}
