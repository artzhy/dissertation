using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PushCommunicator;

namespace WebService {
    public class UserSvc : IUserSvc {

        public BusinessLayer.User AddUser(string forename, string surname, string username, string password) {
            return BusinessLayer.User.CreateUser(forename, surname, username, password);
        }

        public void ModifyUser(AuthToken at, int userId, string forename, string surname, string password) {

            new AuthSvc().AuthUser(at, userId);

            BusinessLayer.User u = BusinessLayer.User.Populate(userId);

            u.Forename = forename;
            u.Password = password;
            u.Surname = surname;

            u.Save();
        }

        public BusinessLayer.UserDevice AddUserDevice(AuthToken at, string deviceType, int deviceMemoryResource, int deviceProcRating, String gcmCode = "") {

            new AuthSvc().AuthUser(at);

            return BusinessLayer.UserDevice.AddUserDevice(at.Username, deviceType, deviceMemoryResource, deviceProcRating, gcmCode);
        }

        public void DeleteUserDevice(AuthToken at, int deviceId) {
            BusinessLayer.UserDevice ud = BusinessLayer.UserDevice.Populate(deviceId);

            new AuthSvc().AuthUser(at, ud.User.UserId);
            
            if (ud.User.Username.Equals(at.Username)) {
                ud.Delete();
            }
        }

        public void ModifyUserDevice(AuthToken at, String gcmCode, int deviceId = -1) {
            if (deviceId != -1) {
               
                new AuthSvc().AuthUser(at, deviceId);
                BusinessLayer.UserDevice ud = BusinessLayer.UserDevice.Populate(deviceId);

                ud.GCMCode = gcmCode;

                ud.Save();
            } else {
                throw new Exception("Device must be selected.");
            }

        }

        public int GetDeviceId(AuthToken at, String gcmId) {
            new AuthSvc().AuthUser(at, -1);
            BusinessLayer.UserDevice ud = BusinessLayer.UserDevice.Populate(gcmId);

            if (BusinessLayer.User.Populate(at.Username).UserId != ud.User.UserId) {
                throw new Exception("Error: Device is not tied to authenticating in user");
            }
            return ud.DeviceId;
        }

        public void SendTestNotification(int deviceId) {
            Pusher push = new Pusher();

            push.SendNotification(BusinessLayer.UserDevice.Populate(deviceId).GCMCode, "{\"alert\":\"Alert Text!\",\"badge\":\"7\"}");

        }

       
    
    }


}
