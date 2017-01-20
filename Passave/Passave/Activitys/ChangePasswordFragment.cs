using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Passave.Activitys
{
    class ChangePasswordFragment : DialogFragment
    {
        private EditText mOldPassword;
        private EditText mNewPassword;
        private EditText mConfirmNewPasswor;
        private Button mConfirm;
        private Button mCancelPassCh;

        public event EventHandler<EditText> eventOldPassword;
        public event EventHandler<EditText> eventNewPassword;
        public event EventHandler<EditText> eventConfirmNewPassword;
        public event EventHandler<Button> eventConfirm;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.ChangePassword, container, false);

            mOldPassword = view.FindViewById<EditText>(Resource.Id.editOldPassword);
            mNewPassword = view.FindViewById<EditText>(Resource.Id.editNewPassword);
            mConfirmNewPasswor = view.FindViewById<EditText>(Resource.Id.editConfirmNewPassword);
            mConfirm = view.FindViewById<Button>(Resource.Id.btnConfirmChPass);
            mCancelPassCh = view.FindViewById<Button>(Resource.Id.btnCancelChPass);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            mOldPassword.AfterTextChanged += MOldPassword_AfterTextChanged;
            mNewPassword.AfterTextChanged += MNewPassword_AfterTextChanged;
            mConfirmNewPasswor.AfterTextChanged += MConfirmNewPasswor_AfterTextChanged;
            mConfirm.Click += MConfirm_Click;
            mCancelPassCh.Click += MCancelPassCh_Click;
            return view;
        }
        private void MConfirm_Click(object sender, EventArgs e)
        {
            if (mNewPassword.Text == mConfirmNewPasswor.Text)
            {
                this.Dismiss();
            }
            eventConfirm.Invoke(this, mConfirm);
        }
        private void MConfirmNewPasswor_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventConfirmNewPassword.Invoke(this, mConfirmNewPasswor);
        }
        private void MNewPassword_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventNewPassword.Invoke(this, mNewPassword);
        }
        private void MOldPassword_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            eventOldPassword.Invoke(this, mOldPassword);
        }
        private void MCancelPassCh_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}