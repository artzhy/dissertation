package computeandroidapp;


public class BroadcastReceiver
	extends gcmsharp.client.GCMBroadcastReceiver_1
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ComputeAndroidApp.BroadcastReceiver, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BroadcastReceiver.class, __md_methods);
	}


	public BroadcastReceiver ()
	{
		super ();
		if (getClass () == BroadcastReceiver.class)
			mono.android.TypeManager.Activate ("ComputeAndroidApp.BroadcastReceiver, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
