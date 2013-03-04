using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    [KnownType(typeof(WorkOrder))]
    [KnownType(typeof(marcdissertation_dbEntities))]
    public partial class CommunicationPackage {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
         [JsonIgnoreAttribute]
        public marcdissertation_dbEntities context;

        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder
        }

        public static CommunicationPackage Populate(int communicationId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
               // App.DbContext.Configuration.ProxyCreationEnabled = false;
                CommunicationPackage comm = (from x in ctxt.CommunicationPackages
                                where x.CommunicationId == communicationId
                        select x).First();
               // App.DbContext.Configuration.ProxyCreationEnabled = true;
                comm.context = ctxt;
                return comm;
            } catch (Exception ex) {
                return null;
            }

        }

        public static CommunicationPackage CreateCommunication(int deviceId, UpdateType commType, int workOrderId) {
            CommunicationPackage comm = new CommunicationPackage();
            comm.TargetDeviceId = deviceId;
            comm.CommunicationType = (int)commType;
            comm.SubmitDate = DateTime.Now;
            comm.WorkOrderId = workOrderId;
            comm.context = new marcdissertation_dbEntities();

            comm = comm.context.CommunicationPackages.Add(comm);
            errors = comm.context.GetValidationErrors();

            try {
                comm.context.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return comm;
        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

        public String Serialize() {

            String serialized = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            return serialized;

        }

        public void Delete() {
           // TODO: Handle if a device is carrying out the WO

            context.CommunicationPackages.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }

        public static List<CommunicationPackage> GetTargetDeviceCommunications(int deviceId) {
            marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
            List<CommunicationPackage> cps = (from x in ctxt.CommunicationPackages where x.TargetDeviceId == 9 && x.Response == null && x.Status == null select x).ToList();
            

            return cps;
        }

    }
}
