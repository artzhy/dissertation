package apptemplate.computefunction;


public class ComputeServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AppTemplate.ComputeFunction.ComputeServiceBinder, com.AppTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ComputeServiceBinder.class, __md_methods);
	}


	public ComputeServiceBinder ()
	{
		super ();
		if (getClass () == ComputeServiceBinder.class)
			mono.android.TypeManager.Activate ("AppTemplate.ComputeFunction.ComputeServiceBinder, com.AppTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ComputeServiceBinder (apptemplate.computefunction.ComputeService p0)
	{
		super ();
		if (getClass () == ComputeServiceBinder.class)
			mono.android.TypeManager.Activate ("AppTemplate.ComputeFunction.ComputeServiceBinder, com.AppTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "AppTemplate.ComputeFunction.ComputeService, com.AppTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
