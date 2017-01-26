using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace Passave
{
    class AddFragment : DialogFragment
    {
        private EditText mUrl;
        private EditText mLogin;
        private EditText mPassword;
        private EditText mDescription;
        private Button mBtnAdd;
        private Button mBtnCancel;
        public event EventHandler<Info> mAddEventAgrs;
        string Url;

        public AddFragment() { }
        public AddFragment(string url)
        {
            Url = url;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.AddItem, container, false);

            mUrl = view.FindViewById<EditText>(Resource.Id.txtUrl);
            mLogin = view.FindViewById<EditText>(Resource.Id.txtLogin);
            mPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mDescription = view.FindViewById<EditText>(Resource.Id.txtDescription);
            mBtnAdd = view.FindViewById<Button>(Resource.Id.btnAdd);
            mBtnCancel = view.FindViewById<Button>(Resource.Id.btnCancelAdd);

            if (Url != null)
            {
                mUrl.Text = Url;
            }

            mBtnAdd.Click += MBtnAdd_Click;
            mBtnCancel.Click += MBtnCancel_Click;
            return view;
        }
        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
        private void MBtnAdd_Click(object sender, EventArgs e)
        {
            if (mUrl.Text == "")
            {
                mAddEventAgrs.Invoke(this, new Info(mUrl.Text, mLogin.Text, mPassword.Text, mDescription.Text));
            }
            else
            {
                mAddEventAgrs.Invoke(this, new Info(mUrl.Text, mLogin.Text, mPassword.Text, mDescription.Text));
                this.Dismiss();
            }
        }
    }
}