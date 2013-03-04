using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class UserDevice {
      [NonSerialized]
        public marcdissertation_dbEntities context;

        public static UserDevice Populate(int deviceId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                UserDevice ud = (from x in ctxt.UserDevices
                                 where x.DeviceId.Equals(deviceId)
                                 select x).First();
                ud.context = ctxt;
                return ud;
            } catch {
                throw new Exception("Device does not exist");
            }
        }

        public static UserDevice Populate(String gcmId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                UserDevice ud = (from x in ctxt.UserDevices
                        where x.GCMCode.Equals(gcmId)
                        select x).First();
                ud.context = ctxt;
                return ud;
            } catch {
                throw new Exception("Device does not exist");
            }
        }

        public static UserDevice AddUserDevice(string username, string deviceType, int deviceMemoryResource, int deviceProcRating) {
            try {
            
                UserDevice ud = new UserDevice();
                ud.Username = username;
                ud.DeviceType = deviceType;
                ud.DeviceMemoryResource = deviceMemoryResource;
                ud.DeviceProcRating = deviceProcRating;
                ud.context =  new marcdissertation_dbEntities();

                ud = ud.context.UserDevices.Add(ud);

                IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = ud.context.GetValidationErrors();

                if (errors.Count() > 0) {
                    throw App.ExceptionFormatter(errors);
                }

                ud.context.SaveChanges();
                return ud;
            } catch (Exception e) {
                if (new marcdissertation_dbEntities().Users.Select(x => x.Username.Equals(username)).Count() == 0) {
                    throw new Exception("Username must exist");
                }

                throw e;
            }

        }

        public static UserDevice AddUserDevice(string username, string deviceType, int deviceMemoryResource, int deviceProcRating, String gcmCode) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                if (ctxt.UserDevices.Count(x => x.GCMCode == gcmCode) > 0) {
                    throw new Exception("Device can only be assigned to single user. Assigned to: " + ctxt.UserDevices.First(x => x.GCMCode == gcmCode).User.Username);
                }

                UserDevice ud = new UserDevice();
                ud.Username = username;
                ud.DeviceType = deviceType;
                ud.DeviceMemoryResource = deviceMemoryResource;
                ud.DeviceProcRating = deviceProcRating;
                ud.GCMCode = gcmCode;
                ud.context = ctxt;
                ud = ud.context.UserDevices.Add(ud);

                IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = ud.context.GetValidationErrors();

                if (errors.Count() > 0) {
                    throw App.ExceptionFormatter(errors);
                }

                ud.context.SaveChanges();
                return ud;
            } catch (Exception e) {
                if (new marcdissertation_dbEntities().Users.Select(x => x.Username.Equals(username)).Count() == 0) {
                    throw new Exception("Username must exist");
                }

                throw e;
            }

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

           context.SaveChanges();

        }

        public void Delete() {
            context.UserDevices.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

    }
}
