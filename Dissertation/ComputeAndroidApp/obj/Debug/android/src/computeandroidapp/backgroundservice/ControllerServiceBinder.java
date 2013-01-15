package computeandroidapp.backgroundservice;


public class ControllerServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ComputeAndroidApp.BackgroundService.ControllerServiceBinder, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ControllerServiceBinder.class, __md_methods);
	}


	public ControllerServiceBinder ()
	{
		super ();
		if (getClass () == ControllerServiceBinder.class)
			mono.android.TypeManager.Activate ("ComputeAndroidApp.BackgroundService.ControllerServiceBinder, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ControllerServiceBinder (computeandroidapp.backgroundservice.ControllerService p0)
	{
		super ();
		if (getClass () == ControllerServiceBinder.class)
			mono.android.TypeManager.Activate ("ComputeAndroidApp.BackgroundService.ControllerServiceBinder, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "ComputeAndroidApp.BackgroundService.ControllerService, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
