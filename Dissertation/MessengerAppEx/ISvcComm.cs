using System;
namespace MessengerAppEx {
    interface ISvcComm {
        Android.OS.Messenger messenger {
            get;
            set;
        }
    }
}
