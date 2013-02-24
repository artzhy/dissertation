using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer {
    public class Scheduler {

        public static UserDevice GetAvailableSlave() {
            UserDevice ud = (from x in App.DbContext.ActiveDevices
                             where x.UserDevice.WorkOrders.Select(y => y.WorkOrderStatus != "RESULT_RECEIVED").Count() < 5
                             orderby x.LastActiveSend ascending
                             select x.UserDevice).First();

            return ud;
        }
    }
}
