using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace Passave.Activitys
{
    [Activity(Label = "AboutActivity")]
    public class AboutActivity : Activity
    {
        private EditText editAbout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About);

            editAbout = FindViewById<EditText>(Resource.Id.editAbout);
            //string s = File.ReadAllText(@"/D:\Sashuteak\Passave\Passave\Passave\About.txt");
            editAbout.Text = "Passave - ���������� ������� ��������� ��������� - ������, ������, �������! � ��� �� ���� �� ���������������� �� ����� - �������� ��� ��������� ���� ����� � ������, � �� ������ ����� � ��� ��� �����!����������� ���������� Passave:���������� ����� ������ � �������������!1.��� �� �������� ������������� ��� ����, ����������:";
        }

        [Export("ReturnClick")]
        public void ReturnClick(View v)
        {
            this.Finish();
        }
    }
}