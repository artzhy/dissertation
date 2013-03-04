using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    public class Scheduler {

        public static UserDevice GetAvailableSlave(int appId) {
            try {
                marcdissertation_dbEntities ctxt = new marcdissertation_dbEntities();

                IQueryable<UserDevice> uds = (from x in ctxt.ActiveDevices
                                 where (x.UserDevice.DeviceAppInstallations.Count(z => z.ApplicationId == appId) > 0) && x.UserDevice.CommunicationPackages.Count(y => y.Status == null && y.Response == null) < App.MAX_COMMS_SLAVE_QUEUED
                                 orderby x.LastFetch ascending
                                 select x.UserDevice);

                return uds.First();
            } catch (Exception e) {
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
