using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    [KnownType(typeof(CommunicationPackage))]
    public partial class WorkOrder {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        public marcdissertation_dbEntities context;
        public static WorkOrder Populate(int workOrderId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

              //  App.DbContext.Configuration.ProxyCreationEnabled = false;
                WorkOrder wo = (from x in ctxt.WorkOrders
                        where x.WorkOrderId == workOrderId
                        select x).First();
                wo.context = ctxt;
               //App.DbContext.Configuration.ProxyCreationEnabled = true;
                               return wo;
            } catch (Exception ex) {
                throw ex;
            }

        }

        public static WorkOrder CreateWorkOrder(int deviceId, int applicationId, int localDeviceRef) {

            WorkOrder wo = new WorkOrder();
            wo.DeviceId = deviceId;
            wo.ApplicationId = applicationId;
            wo.ReceiveTime = DateTime.Now;
            wo.CommPackageJson = "";
            wo.LocalDeviceId = localDeviceRef;
            wo.context = new marcdissertation_dbEntities();


            wo = wo.context.WorkOrders.Add(wo);
            errors = wo.context.GetValidationErrors();

            try {
                wo.context.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return wo;
        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors =context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

           context.SaveChanges();
            

        }


        public void Delete() {
           // TODO: Handle if a device is carrying out the WO

            context.WorkOrders.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }
    }
}
