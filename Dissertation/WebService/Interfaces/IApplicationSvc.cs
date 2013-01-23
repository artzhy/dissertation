using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IApplicationSvc" in both code and config file together.
    [ServiceContract]
    public interface IApplicationSvc {
        [OperationContract]
        [WebGet(UriTemplate = "AddApplication", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        BusinessLayer.WorkApplication AddApplication(String at, string applicationName, string applicationDescription, string applicationCreator, string applicationPackageUrl, string applicationWorkIntent, string applicaitonNamespace, int applicationVersion = 1);

        [OperationContract]
        [WebGet(UriTemplate = "RegisterDeviceToApp", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        BusinessLayer.DeviceAppInstallation RegisterDeviceToApp(String at, int applicationId, int deviceId);

        [OperationContract]
        [WebGet(UriTemplate = "DeregisterDeviceToApp", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void DeregisterDeviceToApp(String at, int applicationId, int deviceId);

    }
}
