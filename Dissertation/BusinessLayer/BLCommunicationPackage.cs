using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    [KnownType(typeof(BusinessLayer.WorkOrder))]
    public partial class CommunicationPackage :IDisposable {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
         
         [NonSerialized]
         [JsonIgnoreAttribute]
        public marcdissertation_dbEntities context;

        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder,
            Cancel,
            FetchRequest
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

        public static CommunicationPackage CreateCommunication(int deviceId, UpdateType commType, int? workOrderId) {
            CommunicationPackage comm = new CommunicationPackage();
            comm.TargetDeviceId = deviceId;
            comm.CommunicationType = (int)commType;
            comm.SubmitDate = DateTime.Now.AddSeconds(-35); // This is a hack, should be a nullable type
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

        public static CommunicationPackage CreateFetchRequest(int deviceId) {
            // Check there isn't one that already exists and isn't acked yet.
            marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
            if (ctxt.CommunicationPackages.Where(x => x.TargetDeviceId == deviceId && x.CommunicationType == (int)UpdateType.FetchRequest && x.Status == null && x.Response == null).Count() == 0) {
                // Create new
                CommunicationPackage comm = new CommunicationPackage();
                comm.TargetDeviceId = deviceId;
                comm.CommunicationType = (int)UpdateType.FetchRequest;
                comm.SubmitDate = DateTime.Now;
                comm.WorkOrderId = null;
                comm.context = ctxt;

                comm = comm.context.CommunicationPackages.Add(comm);
                errors = comm.context.GetValidationErrors();

                try {
                    comm.context.SaveChanges();
                } catch {
                    throw App.ExceptionFormatter(errors);
                }
                return comm;

            } else {
                CommunicationPackage comm =ctxt.CommunicationPackages.Where(x => x.TargetDeviceId == deviceId && x.CommunicationType == (int)UpdateType.FetchRequest && x.Status == null && x.Response == null).First();
                comm.context = ctxt;
                return comm;
            }
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
            List<CommunicationPackage> cps = (from x in ctxt.CommunicationPackages where x.TargetDeviceId == deviceId && x.Response == null && x.Status == null select x).ToList();
            

            return cps;
        }

        public void Dispose() {
            context.Dispose();
        }
    }
 
}
