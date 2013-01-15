using System;
namespace MessengerAppEx2 {
    interface ISvcComm {
        Android.OS.Messenger messenger {
            get;
            set;
        }
    }
}
