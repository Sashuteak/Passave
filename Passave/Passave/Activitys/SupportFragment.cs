using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;


//Check For Git Main Computer!
namespace Passave.Activitys
{
    class SupportFragment : DialogFragment, View.IOnClickListener
    {
        private Button mWrite;
        private Button mBack;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Support, container, false);
            mWrite = view.FindViewById<Button>(Resource.Id.btnWrite);
            mBack = view.FindViewById<Button>(Resource.Id.btnSupBack);

            mWrite.SetOnClickListener(this);
            mBack.SetOnClickListener(this);

            return view;
        }

        public void OnClick(View v)
        {
            switch(v.Id)
            {
                case Resource.Id.btnWrite:
                    var email = new Intent(Intent.ActionSend);
                    email.PutExtra(Intent.ExtraEmail, new string[] { "Passave.App.Help@gmail.com" });
                    email.PutExtra(Intent.ExtraSubject, "For Support");
                    email.SetType("message/rfc822");
                    StartActivity(email);
                    break;
                case Resource.Id.btnSupBack:
                    this.Dismiss();
                    break;
            }
        }
    }
}