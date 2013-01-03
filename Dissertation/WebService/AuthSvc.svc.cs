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
        public Boolean AuthenticateUser(String username, String password) {
            return BusinessLayer.User.AuthenticateUser(username,password);
        }


    }
}
