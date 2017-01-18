using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Views;
using Passave.DataBaseWork;
using System.Collections.Generic;
using Android.Content;
using Newtonsoft.Json;
using Android.Views.InputMethods;

namespace Passave
{
    [Activity(Label = "Passave", MainLauncher = true, Icon = "@drawable/xs", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "text/plain", Label = "Passave")]
    public class MainActivity : Activity, View.IOnClickListener
    {
        private LinearLayout mLinearLayout;
        private Button mSingIn;
        private EditText mPassword;
        private Button mSingUp;
        private ProgressBar mProgressBar;
        private DriverDB data;
        string url;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView (Resource.Layout.Main);
            data = new DriverDB();

            mSingIn = FindViewById<Button>(Resource.Id.btn_SingIn);
            mPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mSingUp = FindViewById<Button>(Resource.Id.btn_SingUp);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mLinearLayout = FindViewById<LinearLayout>(Resource.Id.liner);

            Intent intent = Intent;
            string action = intent.Action;
            object type = intent.GetType();
            url = intent.GetStringExtra(Intent.ExtraText);

            mSingIn.SetOnClickListener(this);
            mSingUp.SetOnClickListener(this);
            mLinearLayout.SetOnClickListener(this);
        }

        //Функция добавления ShortCut на главной странице
        private void AddShortcut()
        {
            var shortcutIntent = new Intent(this, typeof(MainActivity));
            shortcutIntent.SetAction(Intent.ActionMain);

            var iconResource = Intent.ShortcutIconResource.FromContext(
                this, Resource.Drawable.xs);

            var intent = new Intent();
            intent.PutExtra(Intent.ExtraShortcutIntent, shortcutIntent);
            intent.PutExtra(Intent.ExtraShortcutName, "Passave");
            intent.PutExtra(Intent.ExtraShortcutIconResource, iconResource);
            intent.SetAction("com.android.launcher.action.INSTALL_SHORTCUT");
            SendBroadcast(intent);
        }

        private void ActLikeARequest()
        {
            Thread.Sleep(3000);
            RunOnUiThread(() => { mProgressBar.Visibility = ViewStates.Invisible; });
        }
        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btn_SingIn:
                    List<User> list = data.SelectAllFromUser();
                    foreach (var item in list)
                    {
                        if (mPassword.Text == item.Password)
                        {
                            Intent intent = new Intent(this, typeof(ListPageActivity));
                            User user = new User(item.ID, item.Name, item.Password);
                            intent.PutExtra("User", JsonConvert.SerializeObject(user));
                            intent.PutExtra("Url", url);
                            StartActivity(intent);
                            this.Finish();
                            return;
                        }
                    }
                    Toast.MakeText(this, "Вы ввели неправильный пароль!", ToastLength.Short).Show();
                    break;
                case Resource.Id.btn_SingUp:
                    //Pull up dialog
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    RegistrationFragment signUpDialog = new RegistrationFragment();
                    signUpDialog.Show(transaction, "dialog fragment");
                    signUpDialog.mOnSignUpComplete += signUpDialog_mOnSignUpComplete;
                    break;
                case Resource.Id.liner:
                    InputMethodManager input = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
                    input.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
                    break;
            }
        }
        void signUpDialog_mOnSignUpComplete(object sender, User e)
        {
            data.InsertIntoUsers(e.Name, e.Password);
            mProgressBar.Visibility = ViewStates.Visible;
            Thread thread = new Thread(ActLikeARequest);
            thread.Start();
        }
    }
}

