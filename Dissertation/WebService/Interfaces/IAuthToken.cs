using System;
namespace WebService {
    public interface IAuthToken {
        string Password {
            get;
            set;
        }
        string Username {
            get;
            set;
        }
    }
}
