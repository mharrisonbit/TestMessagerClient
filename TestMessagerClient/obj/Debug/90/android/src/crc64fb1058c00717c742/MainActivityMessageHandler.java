package crc64fb1058c00717c742;


public class MainActivityMessageHandler
	extends android.os.Handler
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_handleMessage:(Landroid/os/Message;)V:GetHandleMessage_Landroid_os_Message_Handler\n" +
			"";
		mono.android.Runtime.register ("TestMessagerClient.MainActivityMessageHandler, TestMessagerClient", MainActivityMessageHandler.class, __md_methods);
	}


	public MainActivityMessageHandler ()
	{
		super ();
		if (getClass () == MainActivityMessageHandler.class)
			mono.android.TypeManager.Activate ("TestMessagerClient.MainActivityMessageHandler, TestMessagerClient", "", this, new java.lang.Object[] {  });
	}


	public MainActivityMessageHandler (android.os.Handler.Callback p0)
	{
		super (p0);
		if (getClass () == MainActivityMessageHandler.class)
			mono.android.TypeManager.Activate ("TestMessagerClient.MainActivityMessageHandler, TestMessagerClient", "Android.OS.Handler+ICallback, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public MainActivityMessageHandler (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == MainActivityMessageHandler.class)
			mono.android.TypeManager.Activate ("TestMessagerClient.MainActivityMessageHandler, TestMessagerClient", "Android.OS.Looper, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public MainActivityMessageHandler (android.os.Looper p0, android.os.Handler.Callback p1)
	{
		super (p0, p1);
		if (getClass () == MainActivityMessageHandler.class)
			mono.android.TypeManager.Activate ("TestMessagerClient.MainActivityMessageHandler, TestMessagerClient", "Android.OS.Looper, Mono.Android:Android.OS.Handler+ICallback, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}

	public MainActivityMessageHandler (crc64fb1058c00717c742.MainActivity p0)
	{
		super ();
		if (getClass () == MainActivityMessageHandler.class)
			mono.android.TypeManager.Activate ("TestMessagerClient.MainActivityMessageHandler, TestMessagerClient", "TestMessagerClient.MainActivity, TestMessagerClient", this, new java.lang.Object[] { p0 });
	}


	public void handleMessage (android.os.Message p0)
	{
		n_handleMessage (p0);
	}

	private native void n_handleMessage (android.os.Message p0);

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
