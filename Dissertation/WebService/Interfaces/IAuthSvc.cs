using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthSvc" in both code and config file together.
    [ServiceContract]
    public interface IAuthSvc {
        [OperationContract]
        [WebGet(UriTemplate = "GetAuthToken", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        String GetAuthToken(String username, String password, int deviceId);

        [OperationContract]
        [WebGet(UriTemplate = "AuthenticateUser", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        Boolean AuthenticateUser(String username, String password);
    }
}
