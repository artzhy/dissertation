using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuthSvc" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AuthSvc.svc or AuthSvc.svc.cs at the Solution Explorer and start debugging.
    public class AuthSvc : IAuthSvc {
        private Boolean AuthenticateUser(String username, String password) {
            return BusinessLayer.User.AuthenticateUser(username,password);
        }

        public void AuthUser(AuthToken at, int userId = -1, int deviceId = -1) {
            if (BusinessLayer.User.AuthenticateUser(at.Username, at.Password)) {
                if (userId != -1) {
                    if (BusinessLayer.User.Populate(at.Username).UserId != userId) {
                        throw new Exception("You many only modify your own user");
                    }
                }

                if (deviceId != -1) {
                    if (!((from x in BusinessLayer.User.Populate(at.Username).UserDevices
                           where x.DeviceId == deviceId
                           select x).Count() == 1)) {
                               throw new Exception("You many only modify your own device");
                    }
                }
            } else {
                throw new Exception("Authentication required");
            }
        }


        public AuthToken GetAuthToken(String username, String password) {
            if(AuthenticateUser(username, password))
                return new AuthToken(username, password);
            else
                throw new Exception("Invalid username and password combination");
        }


    }
}
