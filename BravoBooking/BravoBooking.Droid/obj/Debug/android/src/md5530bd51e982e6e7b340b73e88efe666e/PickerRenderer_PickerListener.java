package md5530bd51e982e6e7b340b73e88efe666e;


public class PickerRenderer_PickerListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.PickerRenderer+PickerListener, Xamarin.Forms.Platform.Android, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null", PickerRenderer_PickerListener.class, __md_methods);
	}


	public PickerRenderer_PickerListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PickerRenderer_PickerListener.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.PickerRenderer+PickerListener, Xamarin.Forms.Platform.Android, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
