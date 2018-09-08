package md5b91140b71aacd300338857cebaa38ef2;


public class CCGameView_CCAndroidScreenReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("CocosSharp.CCGameView+CCAndroidScreenReceiver, CocosSharp", CCGameView_CCAndroidScreenReceiver.class, __md_methods);
	}


	public CCGameView_CCAndroidScreenReceiver ()
	{
		super ();
		if (getClass () == CCGameView_CCAndroidScreenReceiver.class)
			mono.android.TypeManager.Activate ("CocosSharp.CCGameView+CCAndroidScreenReceiver, CocosSharp", "", this, new java.lang.Object[] {  });
	}

	public CCGameView_CCAndroidScreenReceiver (md5b91140b71aacd300338857cebaa38ef2.CCGameView p0)
	{
		super ();
		if (getClass () == CCGameView_CCAndroidScreenReceiver.class)
			mono.android.TypeManager.Activate ("CocosSharp.CCGameView+CCAndroidScreenReceiver, CocosSharp", "CocosSharp.CCGameView, CocosSharp", this, new java.lang.Object[] { p0 });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
