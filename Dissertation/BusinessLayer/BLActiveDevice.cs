using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    public partial class ActiveDevice {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        [NonSerialized]
        public marcdissertation_dbEntities context;

            public static ActiveDevice Populate(int deviceId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
              
              //  ad.context  
                //ActiveDevice ad = new ActiveDevice();
                //ad.context = new marcdissertation_dbEntities();

               
                ActiveDevice ad = (from x in ctxt.ActiveDevices
                                   where x.DeviceId == deviceId
                                   select x).First();
                ad.context = ctxt;

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
                ad.context = new marcdissertation_dbEntities();

                ad.DeviceId = deviceId;
                ad.LastActiveSend = DateTime.Now;
                ad.LastFetch = DateTime.Now.AddMinutes(-3);

                ad = ad.context.ActiveDevices.Add(ad);
                errors = ad.context.GetValidationErrors();

                try {
                    ad.context.SaveChanges();
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
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

        public void Delete() {
            // TODO: Handle if a device is carrying out the WO

            context.ActiveDevices.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }
    }
}
