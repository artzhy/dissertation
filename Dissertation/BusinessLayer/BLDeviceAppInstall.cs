using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class DeviceAppInstallation {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public static DeviceAppInstallation Populate(int applicationDevicePairId) {
            try {
                return (from x in App.DbContext.DeviceAppInstallations
                        where x.ApplicationDevicePairId.Equals(applicationDevicePairId)
                        select x).First();
            } catch {
                throw new Exception("Application device pair with ID given does not exist");
            }

        }

        public static DeviceAppInstallation Populate(int applicaitonId, int deviceId) {
            try {
                return (from x in App.DbContext.DeviceAppInstallations
                        where x.DeviceId.Equals(deviceId) && x.ApplicationId.Equals(applicaitonId) select x).First();
            } catch {
                throw new Exception("Application device pair with ID given does not exist");
            }

        }


        public static DeviceAppInstallation CreatePairing(int applicationId, int deviceId) {

            DeviceAppInstallation dai = new DeviceAppInstallation();
            dai.ApplicationId = applicationId;
            dai.DeviceId = deviceId;
            dai.InstallDate = DateTime.Now;

            dai = App.DbContext.DeviceAppInstallations.Add(dai);
            errors = App.DbContext.GetValidationErrors();

            try {
                App.DbContext.SaveChanges();
            } catch(Exception e) {
                throw App.ExceptionFormatter(errors);
            }
            return dai;

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }


        public void Delete() {
           // TODO: Handle if a device still has work to carry out.

            App.DbContext.DeviceAppInstallations.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }
    }
}
