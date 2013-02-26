using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BusinessLayer;

namespace WebService {
    public class SharedFunctions {
        public static void UpdateDeviceLastCommunicated(int deviceId) {

            if (IsDeviceActive(deviceId)) {
                 ActiveDevice ad = ActiveDevice.Populate(deviceId);
                ad.LastActiveSend = DateTime.Now;
                ad.Save();
            } else {
                ActiveDevice.CreateActiveDevice(deviceId);
            }

        }

        public static Boolean IsDeviceActive(int deviceId) {
            return ActiveDevice.DeviceIsActive(deviceId);
        }

    }
}