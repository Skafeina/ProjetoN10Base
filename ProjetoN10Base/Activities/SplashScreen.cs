using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ProjetoN10Base.Activities
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Splash")]
    public class SplashScreen : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent tlLogin = new Intent(this, typeof(LoginActivity));
            StartActivity(tlLogin);

        }
    }
}