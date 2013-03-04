using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class DeviceAppInstallation {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        private marcdissertation_dbEntities context;

        public static DeviceAppInstallation Populate(int applicationDevicePairId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                DeviceAppInstallation dai = (from x in ctxt.DeviceAppInstallations
                        where x.ApplicationDevicePairId.Equals(applicationDevicePairId)
                        select x).First();
                dai.context = ctxt;
                return dai;
            } catch {
                throw new Exception("Application device pair with ID given does not exist");
            }

        }

        public static DeviceAppInstallation Populate(int applicaitonId, int deviceId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                DeviceAppInstallation dai = (from x in ctxt.DeviceAppInstallations
                        where x.DeviceId.Equals(deviceId) && x.ApplicationId.Equals(applicaitonId) select x).First();
                dai.context = ctxt;
                return dai;
            } catch {
                throw new Exception("Application device pair with ID given does not exist");
            }

        }


        public static DeviceAppInstallation CreatePairing(int applicationId, int deviceId) {
            
            DeviceAppInstallation dai = new DeviceAppInstallation();
            dai.ApplicationId = applicationId;
            dai.DeviceId = deviceId;
            dai.InstallDate = DateTime.Now;
            dai.context = new marcdissertation_dbEntities();

            dai = dai.context.DeviceAppInstallations.Add(dai);
            errors = dai.context.GetValidationErrors();

            try {
                dai.context.SaveChanges();
            } catch(Exception e) {
                throw App.ExceptionFormatter(errors);
            }
            return dai;

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }


        public void Delete() {
           // TODO: Handle if a device still has work to carry out.

            context.DeviceAppInstallations.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }
    }
}
