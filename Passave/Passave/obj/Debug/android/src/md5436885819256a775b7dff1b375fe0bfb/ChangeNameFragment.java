package md5436885819256a775b7dff1b375fe0bfb;


public class ChangeNameFragment
	extends android.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Passave.Activitys.ChangeNameFragment, Passave, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ChangeNameFragment.class, __md_methods);
	}


	public ChangeNameFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ChangeNameFragment.class)
			mono.android.TypeManager.Activate ("Passave.Activitys.ChangeNameFragment, Passave, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ChangeNameFragment (java.lang.String p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ChangeNameFragment.class)
			mono.android.TypeManager.Activate ("Passave.Activitys.ChangeNameFragment, Passave, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

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
