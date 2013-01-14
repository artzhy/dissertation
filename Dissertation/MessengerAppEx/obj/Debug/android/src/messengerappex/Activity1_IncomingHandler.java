package messengerappex;


public class Activity1_IncomingHandler
	extends android.os.Handler
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_handleMessage:(Landroid/os/Message;)V:GetHandleMessage_Landroid_os_Message_Handler\n" +
			"";
		mono.android.Runtime.register ("MessengerAppEx.Activity1/IncomingHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Activity1_IncomingHandler.class, __md_methods);
	}


	public Activity1_IncomingHandler ()
	{
		super ();
		if (getClass () == Activity1_IncomingHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.Activity1/IncomingHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public Activity1_IncomingHandler (android.os.Handler.Callback p0)
	{
		super (p0);
		if (getClass () == Activity1_IncomingHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.Activity1/IncomingHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Handler/ICallback, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0 });
	}


	public Activity1_IncomingHandler (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == Activity1_IncomingHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.Activity1/IncomingHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Looper, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0 });
	}


	public Activity1_IncomingHandler (android.os.Looper p0, android.os.Handler.Callback p1)
	{
		super (p0, p1);
		if (getClass () == Activity1_IncomingHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.Activity1/IncomingHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Looper, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd:Android.OS.Handler/ICallback, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0, p1 });
	}


	public void handleMessage (android.os.Message p0)
	{
		n_handleMessage (p0);
	}

	private native void n_handleMessage (android.os.Message p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
