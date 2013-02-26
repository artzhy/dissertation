using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PushCommunicator;
using BusinessLayer;

namespace WebService {
    public class UserSvc : IUserSvc {

        public BusinessLayer.User AddUser(string forename, string surname, string username, string password) {
            return BusinessLayer.User.CreateUser(forename, surname, username, password);
        }

        public void ModifyUser(String at, int userId, string forename, string surname, string password) {

            AuthenticationToken oAt = new AuthSvc().AuthUser(at, userId);

            BusinessLayer.User u = BusinessLayer.User.Populate(userId);

            u.Forename = forename;
            u.Password = password;
            u.Surname = surname;

            u.Save();
        }

        public BusinessLayer.UserDevice AddUserDeviceNoGCMCode(String at, String username, string deviceType, int deviceMemoryResource, int deviceProcRating) {

            //TODO: Fix auth here
//            AuthenticationToken oAt = new AuthSvc().AuthUser(at);

            return BusinessLayer.UserDevice.AddUserDevice(username, deviceType, deviceMemoryResource, deviceProcRating);
        }


        public BusinessLayer.UserDevice AddUserDevice(String at, string deviceType, int deviceMemoryResource, int deviceProcRating, String gcmCode = "") {
            AuthenticationToken oAt = new AuthSvc().AuthUser(at);


            return BusinessLayer.UserDevice.AddUserDevice(oAt.Username, deviceType, deviceMemoryResource, deviceProcRating, gcmCode);
        }

        public void DeleteUserDevice(String at, int deviceId) {
            BusinessLayer.UserDevice ud = BusinessLayer.UserDevice.Populate(deviceId);

            AuthenticationToken oAt = new AuthSvc().AuthUser(at, ud.User.UserId);

            if (ud.User.Username.Equals(oAt.Username)) {
                ud.Delete();
            }
        }

        public void ModifyUserDevice(String at, String gcmCode, int deviceId = -1) {
            if (deviceId != -1) {

                BusinessLayer.UserDevice ud = BusinessLayer.UserDevice.Populate(deviceId);

                ud.GCMCode = gcmCode;

                ud.Save();
            } else {
                throw new Exception("Device must be selected.");
            }

        }

        public int GetDeviceId(String at, String gcmId) {
            AuthenticationToken oAt = new AuthSvc().AuthUser(at);

            BusinessLayer.UserDevice ud = BusinessLayer.UserDevice.Populate(gcmId);

            if (BusinessLayer.User.Populate(oAt.Username).UserId != ud.User.UserId) {
                throw new Exception("Error: Device is not tied to authenticating in user");
            }
            return ud.DeviceId;
        }

        public void SendTestNotification(int deviceId) {
            Pusher push = new Pusher();

            push.SendNotification(BusinessLayer.UserDevice.Populate(deviceId).GCMCode, "{\"WorkOrderId\":\"12\"}");

        }

        public void MarkDeviceActive(string at, int deviceId) {
            try {
                AuthenticationToken oAt = new AuthSvc().AuthUser(at, -1, deviceId);
            } catch(Exception e) {
                String ex = e.Message;
            }

          
        }

        public void MarkDeviceInactive(string at, int deviceId) {
            AuthenticationToken oAt = new AuthSvc().AuthUser(at, -1, deviceId);

            try {
                ActiveDevice ad = ActiveDevice.Populate(deviceId);
                ad.Delete();
                //TODO: Re-assign work orders
            } catch {

            }

        }

    }


}
