using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Passave
{
    class RegistrationFragment : DialogFragment
    {
        private EditText mTxtFirstName;
        private EditText mTxtPassword;
        private Button mBtnSignUp;
        public event EventHandler<User> mOnSignUpComplete;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Registration, container, false);

            mTxtFirstName = view.FindViewById<EditText>(Resource.Id.txtName);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mBtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogReg);

            mBtnSignUp.Click += mBtnSignUp_Click;
            return view;
        }
        void mBtnSignUp_Click(object sender, EventArgs e)
        {
           //User has clicked the sign up button
            mOnSignUpComplete.Invoke(this, new User(mTxtFirstName.Text, mTxtPassword.Text));
            this.Dismiss();
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; //set the animation
        }
    }
}