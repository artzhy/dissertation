using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    public partial class ActiveDevice {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public static ActiveDevice Populate(int deviceId) {
            try {
            
                ActiveDevice ad = (from x in App.DbContext.ActiveDevices
                                   where x.DeviceId == deviceId
                                   select x).First();
                 return ad;
            } catch {
                throw new Exception("Device not active");
            }

        }

        public static ActiveDevice CreateActiveDevice(int deviceId) {
            ActiveDevice ad = null;
            try {
                ad = Populate(deviceId);
            } catch {
                // Device not active, actiavate it

                ad = new ActiveDevice();
                ad.DeviceId = deviceId;
                ad.LastActiveSend = DateTime.Now;

                ad = App.DbContext.ActiveDevices.Add(ad);
                errors = App.DbContext.GetValidationErrors();

                try {
                    App.DbContext.SaveChanges();
                } catch {
                    throw App.ExceptionFormatter(errors);
                }

            }

            return ad;
        }

        public static Boolean DeviceIsActive(int deviceId) {
            try {
                Populate(deviceId);
                return true;
            } catch (Exception) {

            }

            return false;
        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

        public void Delete() {
            // TODO: Handle if a device is carrying out the WO

            App.DbContext.ActiveDevices.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }
    }
}
