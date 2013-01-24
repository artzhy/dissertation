using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable]
    public partial class WorkOrder {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public static WorkOrder Populate(int workOrderId) {
            try {
                App.DbContext.Configuration.ProxyCreationEnabled = false;
                WorkOrder wo = (from x in App.DbContext.WorkOrders
                        where x.WorkOrderId == workOrderId
                        select x).First();
                App.DbContext.Configuration.ProxyCreationEnabled = true;
                return wo;
            } catch {
                throw new Exception("Application device pair with ID given does not exist");
            }

        }

        public static WorkOrder CreateWorkOrder(int deviceId, int applicationId, String commPackageJson) {

            WorkOrder wo = new WorkOrder();
            wo.DeviceId = deviceId;
            wo.ApplicationId = applicationId;
            wo.CommPackageJson = commPackageJson;
            wo.ReceiveTime = DateTime.Now;

            wo = App.DbContext.WorkOrders.Add(wo);
            errors = App.DbContext.GetValidationErrors();

            try {
                App.DbContext.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return wo;
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

            App.DbContext.WorkOrders.Remove(this);

            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }
    }
}
