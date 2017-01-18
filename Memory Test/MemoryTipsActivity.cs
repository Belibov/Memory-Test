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

namespace Memory_Test
{
    [Activity(Label = "MemoryTipsActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/ic_launcher")]
    public class MemoryTipsActivity : Activity
    {
        private List<String> list;
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.MemoryTips);

            listView = FindViewById<ListView>(Resource.Id.listView1);

            list = new List<string>();
            list.Add("Give your brain a workout");
            list.Add("Don't skip the physical exercise");
            list.Add("Get your Zs");
            list.Add("Make time for friends");
            list.Add("Keep stress in check");
            list.Add("Have a laugh");
            list.Add("Eat a brain-boosting diet");
            list.Add("Identify and treat health problems");
            list.Add("Take practical steps to support learning and memory");

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, list);
            listView.Adapter = adapter;


        }
    }
}