using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    [Serializable()]
    public partial class WorkApplication {
        private static IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors;

        public static WorkApplication Populate(int ApplicationId) {
            try {
                return (from x in App.DbContext.WorkApplications
                        where x.ApplicationId.Equals(ApplicationId)
                        select x).First();
            } catch {
                throw new Exception("Application with ID given does not exist");
            }

        }

        public static WorkApplication Populate(String applicationName) {
            try {
                return (from x in App.DbContext.WorkApplications
                        where x.ApplicationName == applicationName
                        select x).First();
            } catch {
                throw new Exception("Application with name given does not exist");
            }
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

            wa = App.DbContext.WorkApplications.Add(wa);
            errors = App.DbContext.GetValidationErrors();

            try {
                App.DbContext.SaveChanges();
            } catch {
                throw App.ExceptionFormatter(errors);
            }
            return wa;

        }

        public void Save() {
            IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> errors = App.DbContext.GetValidationErrors();

            if (errors.Count() > 0) {
                throw App.ExceptionFormatter(errors);
            }

            App.DbContext.SaveChanges();

        }
    }
}
