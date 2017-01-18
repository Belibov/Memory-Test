using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Memory_Test
{
    [Activity(Label = "Memory Test", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity
    {
        private Button btnStartTest;
        private Button btnMyResults;
        private Button btnMemoryTips;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            btnMyResults = FindViewById<Button>(Resource.Id.btnMyResult);
            btnMyResults.Click += BtnMyResults_Click;

            btnMemoryTips = FindViewById<Button>(Resource.Id.btnMemoryTips);
            btnMemoryTips.Click += BtnMemoryTips_Click;

            btnStartTest = FindViewById<Button>(Resource.Id.btnStartTest);
            btnStartTest.Click += BtnStartTest_Click;
        }

        private void BtnStartTest_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(TestActivity));
        }

        private void BtnMemoryTips_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MemoryTipsActivity));
        }

        private void BtnMyResults_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MyScoreActivity));
        }
    }
}