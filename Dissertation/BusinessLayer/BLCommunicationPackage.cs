using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    [KnownType(typeof(WorkOrder))]
    public partial class CommunicationPackage {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder
        }

        public static CommunicationPackage Populate(int communicationId) {
            try {
               // App.DbContext.Configuration.ProxyCreationEnabled = false;
                CommunicationPackage comm = (from x in App.DbContext.CommunicationPackages
                                where x.CommunicationId == communicationId
                        select x).First();
               // App.DbContext.Configuration.ProxyCreationEnabled = true;
                return comm;
            } catch (Exception ex) {
                throw ex;
            }

        }

        public static CommunicationPackage CreateCommunication(int deviceId, UpdateType commType, int workOrderId) {

            CommunicationPackage comm = new CommunicationPackage();
            comm.TargetDeviceId = deviceId;
            comm.CommunicationType = (int)commType;
            comm.SubmitDate = DateTime.Now;
            comm.WorkOrderId = workOrderId;
  
            comm = App.DbContext.CommunicationPackages.Add(comm);
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

        public String Serialize() {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this);

        }

        public void Delete() {
           // TODO: Handle if a device is carrying out the WO

            App.DbContext.CommunicationPackages.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }

    }
}
