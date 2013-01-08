package messengerappex;


public class CHandler
	extends android.os.Handler
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MessengerAppEx.CHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CHandler.class, __md_methods);
	}


	public CHandler ()
	{
		super ();
		if (getClass () == CHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.CHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public CHandler (android.os.Handler.Callback p0)
	{
		super (p0);
		if (getClass () == CHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.CHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Handler/ICallback, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0 });
	}


	public CHandler (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == CHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.CHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Looper, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0 });
	}


	public CHandler (android.os.Looper p0, android.os.Handler.Callback p1)
	{
		super (p0, p1);
		if (getClass () == CHandler.class)
			mono.android.TypeManager.Activate ("MessengerAppEx.CHandler, MessengerAppEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Looper, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd:Android.OS.Handler/ICallback, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=c4c4237547e4b6cd", this, new java.lang.Object[] { p0, p1 });
	}

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
