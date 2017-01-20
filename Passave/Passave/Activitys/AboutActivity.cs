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
using Java.Util;
using System.IO;

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
           


            // Create your application here
        }
    }
}