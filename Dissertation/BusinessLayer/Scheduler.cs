using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    public class Scheduler {

        public static UserDevice GetAvailableSlave(int appId) {
            try {

                UserDevice ud = (from x in App.DbContext.ActiveDevices
                                 where x.UserDevice.DeviceAppInstallations.Select(z => z.ApplicationId == appId).Count() > 0
                                 orderby x.LastActiveSend ascending
                                 select x.UserDevice).First();

                //(x.UserDevice.WorkOrders.Select(y => y.WorkOrderStatus != "RESULT_RECEIVED").Count() < 5) && 

                return ud;
            } catch (Exception) {
                throw new Exception("No Device Available");
            }
        }

        public static List<CommunicationPackage> GetUnacknowledgedPackages(int timePeriodSecs) {
            DateTime dateToUse = DateTime.Now.AddSeconds(-timePeriodSecs);

           
            return (from x in App.DbContext.CommunicationPackages
                    where x.DateAcknowledged == null && x.Status == null && x.SubmitDate <= dateToUse  
                    select x).ToList();

         
        }



     
    }
}
