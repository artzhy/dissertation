using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService {
    public class ApplicationSvc : IApplicationSvc {

        public BusinessLayer.WorkApplication AddApplication(String at, string applicationName, string applicationDescription, string applicationCreator, string applicationPackageUrl, string applicationWorkIntent, string applicationNamespace, int applicationVersion = 1) {

            new AuthSvc().AuthUser(at);
                
            return BusinessLayer.WorkApplication.CreateApplication(applicationName, applicationDescription, applicationCreator, applicationPackageUrl, applicationWorkIntent, applicationNamespace, applicationVersion);
        }

        public BusinessLayer.DeviceAppInstallation RegisterDeviceToApp(String at, int applicationId, int deviceId) {

            new AuthSvc().AuthUser(at, -1, deviceId);

            return BusinessLayer.DeviceAppInstallation.CreatePairing(applicationId, deviceId);
        }

        public void DeregisterDeviceToApp(String at, int applicationId, int deviceId) {
            new AuthSvc().AuthUser(at, -1, deviceId);

            BusinessLayer.DeviceAppInstallation toRemove = BusinessLayer.DeviceAppInstallation.Populate(applicationId, deviceId);

            toRemove.Delete();
        }


    }
}
