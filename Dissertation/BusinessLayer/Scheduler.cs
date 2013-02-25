using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    public class Scheduler {

        public static UserDevice GetAvailableSlave(int appId) {
            UserDevice ud = (from x in App.DbContext.ActiveDevices
                             where (x.UserDevice.WorkOrders.Select(y => y.WorkOrderStatus != "RESULT_RECEIVED").Count() < 5) && x.UserDevice.DeviceAppInstallations.Select(z => z.ApplicationId == appId).Count() == 1
                             orderby x.LastActiveSend ascending
                             select x.UserDevice).First();

            return ud;
        }

        public static List<CommunicationPackage> GetUnacknowledgedPackages(int timePeriodSecs) {
            return (from x in App.DbContext.CommunicationPackages
                    where x.DateAcknowledged == null && x.SubmitDate <= DateTime.Now.AddSeconds(-timePeriodSecs)
                                                                    select x).ToList();
        }



     
    }
}
