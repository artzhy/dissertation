using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebService {
  
    public class AuthSvc : IAuthSvc {
        public Boolean AuthenticateUser(String username, String password) {
            return BusinessLayer.User.AuthenticateUser(username,password);
        }

        private BusinessLayer.AuthenticationToken AuthoriseUser(string at) {
            try {
                return BusinessLayer.AuthenticationToken.Populate(at);
            } catch {
                return null;
            }
        }

        public BusinessLayer.AuthenticationToken AuthUser(string at, int userId = -1, int deviceId = -1) {
            BusinessLayer.AuthenticationToken aut = AuthoriseUser(at);

            if (aut != null) {
                if (userId != -1) {
                    if (aut.User.UserId != userId) {
                        throw new Exception("You many only modify your own user");
                    }
                }

                if (deviceId != -1) {
                    if (aut.DeviceId != deviceId) {
                        throw new Exception("You many only modify your own device");
                    }

                    //if (!((from x in BusinessLayer.User.Populate(at.Username).UserDevices
                    //       where x.DeviceId == deviceId
                    //       select x).Count() == 1)) {
                    //           throw new Exception("You many only modify your own device");
                    //}
                }
            } else {
                throw new Exception("Authentication required");
            }
            return aut;
        }


        public String GetAuthToken(String username, String password, int deviceId) {
            if(AuthenticateUser(username, password)) {
                BusinessLayer.AuthenticationToken at = null;
                try {
                    at = BusinessLayer.AuthenticationToken.Populate(deviceId);
                } catch {}

                if (at == null) {
                    // Generate new auth token.
                    at = BusinessLayer.AuthenticationToken.AddAuthenticationToken(deviceId, username);
                }

                return at.Token;
            } else
                throw new Exception("Invalid username and password combination");
        }


    }
}
