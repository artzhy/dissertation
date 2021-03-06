﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class WorkApplication {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;
        [NonSerialized]
        private marcdissertation_dbEntities context;


        public static WorkApplication Populate(int ApplicationId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                WorkApplication wa = (from x in ctxt.WorkApplications
                        where x.ApplicationId.Equals(ApplicationId)
                        select x).First();
                wa.context = ctxt;
                return wa;
            } catch {
                throw new Exception("Application with ID given does not exist");
            }

        }

        public static WorkApplication Populate(String applicationName) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                WorkApplication wa =  (from x in ctxt.WorkApplications
                        where x.ApplicationName == applicationName
                        select x).First();
                wa.context = ctxt;
                return wa;
            } catch {
                throw new Exception("Application with name given does not exist");
            }
        }

        public double GetAverageComputeTime() {
            double sum = 0;
            int count = 0;

            foreach(WorkOrder wo in context.WorkOrders.Where(x => x.WorkOrderStatus == "RESULT_RECEIVED").Take(15)) {
                sum += wo.SlaveWorkOrderLastCommunication.Value.Subtract(wo.SlaveWorkerSubmit.Value).Seconds;
                count++;
            }

            if (sum == 0 || count == 0) {
                return 0;
            }


            return sum/count;
        }

        public static WorkApplication CreateApplication(string applicationName, string applicationDescription, string applicationCreator, string applicationPackageUrl, string applicationWorkIntent, string applicationNamespace, int applicationVersion = 1) {

            WorkApplication wa = new WorkApplication();
            wa.ApplicationName = applicationName;
            wa.ApplicationCreator = applicationCreator;
            wa.ApplicationDescription = applicationDescription;
            wa.ApplicationPackageURL = applicationPackageUrl;
            wa.ApplicationWorkIntent = applicationWorkIntent;
            wa.ApplicationVersion = applicationVersion;
            wa.ApplicationNamespace = applicationNamespace;
            wa.context = new marcdissertation_dbEntities();

            wa = wa.context.WorkApplications.Add(wa);
            errors = wa.context.GetValidationErrors();

            try {
                wa.context.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return wa;

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = context.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            context.SaveChanges();

        }
    }
}
