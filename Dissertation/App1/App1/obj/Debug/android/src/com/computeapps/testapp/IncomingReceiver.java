package com.computeapps.testapp;


public class IncomingReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("com.ComputeApps.TestApp.IncomingReceiver, com.ComputeApps.TestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", IncomingReceiver.class, __md_methods);
	}


	public IncomingReceiver ()
	{
		super ();
		if (getClass () == IncomingReceiver.class)
			mono.android.TypeManager.Activate ("com.ComputeApps.TestApp.IncomingReceiver, com.ComputeApps.TestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
