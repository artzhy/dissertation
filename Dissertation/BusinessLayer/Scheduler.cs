using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    public class Scheduler {

        public static UserDevice GetAvailableSlave(int appId, int deviceId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

                //TODO: change this from commm package to work order
                IQueryable<UserDevice> uds = (from x in ctxt.ActiveDevices
                                 where (x.UserDevice.DeviceAppInstallations.Count(z => z.ApplicationId == appId) > 0) && x.UserDevice.WorkOrders1.Count(y => y.WorkOrderStatus != "RESULT_RECEIVED") < App.MAX_COMMS_SLAVE_QUEUED && x.DeviceId != deviceId orderby x.LastFetch ascending
                                 select x.UserDevice);

                return uds.First();
            } catch (Exception) {
                throw new Exception("No Device Available");
            }
        }

        public static List<CommunicationPackage> GetUnacknowledgedPackages(int timePeriodSecs) {
            DateTime dateToUse = DateTime.Now.AddSeconds(-timePeriodSecs);
            marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

            List<CommunicationPackage> unAcked = (from x in ctxt.CommunicationPackages
                                                  where x.DateAcknowledged == null && x.Status == null && x.SubmitDate <= dateToUse
                                                  select x).ToList();

            return unAcked;

         
        }



     
    }
}
