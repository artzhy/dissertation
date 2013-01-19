using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebService {
    [Serializable]
    public class AuthToken {
        public String Username {
            get;
            set;
        }

        public String Password {
            get;
            set;
        }

        public AuthToken(String username, String password) {
            Username = username;
            Password = password;
        }

       //TODO: Implement encryption of password.
    }
}