using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Passave.Activitys
{
    class EditInfoFragment : DialogFragment
    {
        private EditText mUrl;
        private EditText mLogin;
        private EditText mPassword;
        private EditText mDescription;
        private Button mConfirm;
        private Button mCancel;

        public event EventHandler<EditText> eventUrl;
        public event EventHandler<EditText> eventLogin;
        public event EventHandler<EditText> eventPassword;
        public event EventHandler<EditText> eventDescription;
        public event EventHandler<Button> eventConfirm;

        private string Url;
        private string Login;
        private string Password;
        private string Description;

        public EditInfoFragment(string url, string login, string password, string description)
        {
            Url = url;
            Login = login;
            Password = password;
            Description = description;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.EditItem, container, false);

            mUrl = view.FindViewById<EditText>(Resource.Id.editUrl);
            mLogin = view.FindViewById<EditText>(Resource.Id.editLogin);
            mPassword = view.FindViewById<EditText>(Resource.Id.editPassword);
            mDescription = view.FindViewById<EditText>(Resource.Id.editDescription);
            mConfirm = view.FindViewById<Button>(Resource.Id.btnConfirmEdit);
            mCancel = view.FindViewById<Button>(Resource.Id.btnEditCanceled);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            mUrl.Text = Url;
            mLogin.Text = Login;
            mPassword.Text = Password;
            mDescription.Text = Description;

            mUrl.AfterTextChanged += MUrl_AfterTextChanged;
            mLogin.AfterTextChanged += MLogin_AfterTextChanged;
            mPassword.AfterTextChanged += MPassword_AfterTextChanged;
            mDescription.AfterTextChanged += MDescription_AfterTextChanged;

            mConfirm.Click += MConfirm_Click;
            mCancel.Click += MCancel_Click;
            return view;
        }

        private void MDescription_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventDescription.Invoke(this, mDescription);
        }
        private void MPassword_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventPassword.Invoke(this, mPassword);
        }
        private void MLogin_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventLogin.Invoke(this, mLogin);
        }
        private void MUrl_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventUrl.Invoke(this, mUrl);
        }
        private void MConfirm_Click(object sender, EventArgs e)
        {
            eventConfirm.Invoke(this, mConfirm);
            this.Dismiss();
        }
        private void MCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}