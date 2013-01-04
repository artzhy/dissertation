package computeandroidapp;


public class GCMIntentService
	extends gcmsharp.client.GCMBaseIntentService
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ComputeAndroidApp.GCMIntentService, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GCMIntentService.class, __md_methods);
	}


	public GCMIntentService (java.lang.String p0)
	{
		super (p0);
		if (getClass () == GCMIntentService.class)
			mono.android.TypeManager.Activate ("ComputeAndroidApp.GCMIntentService, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public GCMIntentService ()
	{
		super ();
		if (getClass () == GCMIntentService.class)
			mono.android.TypeManager.Activate ("ComputeAndroidApp.GCMIntentService, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
