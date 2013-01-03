using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IUserSvc {
        [OperationContract]
        [WebGet(UriTemplate = "AddUser", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        BusinessLayer.User AddUser(string forename, string surname, string username, string password);

        [OperationContract]
        [WebGet(UriTemplate = "ModifyUser", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void ModifyUser(String authUsername, string authPassword, int userId, string forename, string surname, string password);

        [OperationContract]
        [WebGet(UriTemplate = "AddUserDevice", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        BusinessLayer.UserDevice AddUserDevice(string authUsername, String authPassword, string deviceType, int deviceMemoryResource, int deviceProcRating);

        [OperationContract]
        [WebGet(UriTemplate = "DeleteUserDevice", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void DeleteUserDevice(string authUsername, String authPassword, int deviceId);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
