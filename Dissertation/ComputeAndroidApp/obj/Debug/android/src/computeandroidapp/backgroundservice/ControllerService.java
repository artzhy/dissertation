package computeandroidapp.backgroundservice;


public class ControllerService
	extends android.app.Service
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("ComputeAndroidApp.BackgroundService.ControllerService, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ControllerService.class, __md_methods);
	}


	public ControllerService ()
	{
		super ();
		if (getClass () == ControllerService.class)
			mono.android.TypeManager.Activate ("ComputeAndroidApp.BackgroundService.ControllerService, ComputeAndroidApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.os.IBinder onBind (android.content.Intent p0)
	{
		return n_onBind (p0);
	}

	private native android.os.IBinder n_onBind (android.content.Intent p0);

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
