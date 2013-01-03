using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class UserDevice {

        public static UserDevice Populate(int deviceId) {
            try {
                return (from x in App.DbContext.UserDevices
                        where x.DeviceId.Equals(deviceId)
                        select x).First();
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

                ud = App.DbContext.UserDevices.Add(ud);

                IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

                if (errors.Count() > 0) {
                    throw App.ExceptionFormatter(errors);
                }

                App.DbContext.SaveChanges();
                return ud;
            } catch (Exception e) {
                if (App.DbContext.Users.Select(x => x.Username.Equals(username)).Count() == 0) {
                    throw new Exception("Username must exist");
                }

                throw e;
            }

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

        public void Delete() {
            App.DbContext.UserDevices.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

    }
}
