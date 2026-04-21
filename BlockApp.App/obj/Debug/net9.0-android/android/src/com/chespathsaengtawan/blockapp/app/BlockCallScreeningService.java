package com.chespathsaengtawan.blockapp.app;


public class BlockCallScreeningService
	extends android.telecom.CallScreeningService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScreenCall:(Landroid/telecom/Call$Details;)V:GetOnScreenCall_Landroid_telecom_Call_Details_Handler\n" +
			"";
		mono.android.Runtime.register ("BlockApp.App.Platforms.Android.Services.BlockCallScreeningService, BlockApp.App", BlockCallScreeningService.class, __md_methods);
	}

	public BlockCallScreeningService ()
	{
		super ();
		if (getClass () == BlockCallScreeningService.class) {
			mono.android.TypeManager.Activate ("BlockApp.App.Platforms.Android.Services.BlockCallScreeningService, BlockApp.App", "", this, new java.lang.Object[] {  });
		}
	}

	public void onScreenCall (android.telecom.Call.Details p0)
	{
		n_onScreenCall (p0);
	}

	private native void n_onScreenCall (android.telecom.Call.Details p0);

	private java.util.ArrayList refList;
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
