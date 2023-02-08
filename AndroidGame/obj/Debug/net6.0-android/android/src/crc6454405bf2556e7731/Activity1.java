package crc6454405bf2556e7731;


public class Activity1
	extends crc64493ac3851fab1842.AndroidGameActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onWindowFocusChanged:(Z)V:GetOnWindowFocusChanged_ZHandler\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("AndroidGame.Activity1, Nightmare", Activity1.class, __md_methods);
	}


	public Activity1 ()
	{
		super ();
		if (getClass () == Activity1.class)
			mono.android.TypeManager.Activate ("AndroidGame.Activity1, Nightmare", "", this, new java.lang.Object[] {  });
	}


	public void onWindowFocusChanged (boolean p0)
	{
		n_onWindowFocusChanged (p0);
	}

	private native void n_onWindowFocusChanged (boolean p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
