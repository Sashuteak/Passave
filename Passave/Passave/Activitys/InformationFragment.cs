using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace Passave.Activitys
{
    [Activity(Label = "InformationActivity")]
    public class InformationFragment : DialogFragment
    {
        private Button mSubmit;
        private Button mBack;
        private TextView mUrl;
        private TextView mLogin;
        private TextView mPassword;
        private TextView mDescription;
        private Info info;

        public InformationFragment(string url, string login, string password, string description)
        {
            info = new Info(url, login, password, description);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Information, container, false);

            mBack = view.FindViewById<Button>(Resource.Id.btnBack);
            mSubmit = view.FindViewById<Button>(Resource.Id.btnSubmitGo);
            mUrl = view.FindViewById<TextView>(Resource.Id.textUrl);
            mLogin = view.FindViewById<TextView>(Resource.Id.textLogin);
            mPassword = view.FindViewById<TextView>(Resource.Id.textPassword);
            mDescription = view.FindViewById<TextView>(Resource.Id.editDescription);

            Dialog.Window.SetTitle("Информация О Сайте");
            mUrl.Text = info.Name;
            mLogin.Text = info.Login;
            mPassword.Text = info.Password;
            mDescription.Text = info.Description;

            mSubmit.Click += MSubmit_Click;
            mBack.Click += MBack_Click;
            return view;
        }

        private void MBack_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        private void MSubmit_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse(info.Name);
            Intent intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
    }
}