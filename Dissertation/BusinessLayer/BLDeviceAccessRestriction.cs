using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    public partial class DeviceAccessRestriction {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        [NonSerialized]
        private marcdissertation_dbEntities context;

        public static DeviceAccessRestriction Populate(int accessRestrictionId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

                DeviceAccessRestriction dar = (from x in ctxt.DeviceAccessRestrictions
                                where x.AccessRestrictionId == accessRestrictionId
                        select x).First();
                dar.context = ctxt;
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
            dar.context = new marcdissertation_dbEntities();


            dar = dar.context.DeviceAccessRestrictions.Add(dar);
            errors = dar.context.GetValidationErrors();

            try {
                dar.context.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return dar;
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

            context.DeviceAccessRestrictions.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }
    }
}
