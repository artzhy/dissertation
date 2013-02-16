using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    public partial class Communication {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public static Communication Populate(int communicationId) {
            try {
               // App.DbContext.Configuration.ProxyCreationEnabled = false;
                Communication comm = (from x in App.DbContext.Communications
                                where x.CommunicationId == communicationId
                        select x).First();
               // App.DbContext.Configuration.ProxyCreationEnabled = true;
                return comm;
            } catch (Exception ex) {
                throw ex;
            }

        }

        public static Communication CreateCommunication(int deviceId, String commType) {

            Communication comm = new Communication();
            comm.TargetDeviceId = deviceId;
            comm.CommunicationType = commType;
            comm.SubmitDate = DateTime.Now;

            comm = App.DbContext.Communications.Add(comm);
            errors = App.DbContext.GetValidationErrors();

            try {
                App.DbContext.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return comm;
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

            App.DbContext.Communications.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }
    }
}
