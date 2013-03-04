using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    public class Scheduler {

        public static UserDevice GetAvailableSlave(int appId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();
                UserDevice ud = (from x in ctxt.ActiveDevices
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
            marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

            return (from x in ctxt.CommunicationPackages
                    where x.DateAcknowledged == null && x.Status == null && x.SubmitDate <= dateToUse  
                    select x).ToList();

         
        }



     
    }
}
