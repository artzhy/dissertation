using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    public partial class DeviceAccessRestriction {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public static DeviceAccessRestriction Populate(int accessRestrictionId) {
            try {
                DeviceAccessRestriction dar = (from x in App.DbContext.DeviceAccessRestrictions
                                where x.AccessRestrictionId == accessRestrictionId
                        select x).First();

                return dar;
            } catch (Exception ex) {
                throw ex;
            }

        }

        public static DeviceAccessRestriction CreateDeviceAccessRestriction(int deviceId, DayOfWeek dow, TimeSpan startTime, TimeSpan endTime) {

            DeviceAccessRestriction dar = new DeviceAccessRestriction();
            dar.DeviceId = deviceId;
            dar.Day = (int) dow;
            dar.StartTime = startTime;
            dar.EndTime = endTime;


            dar = App.DbContext.DeviceAccessRestrictions.Add(dar);
            errors = App.DbContext.GetValidationErrors();

            try {
                App.DbContext.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return dar;
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

            App.DbContext.DeviceAccessRestrictions.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }
    }
}
