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
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using System.IO;

namespace Memory_Test
{
    [Activity(Label = "MyScoreActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/ic_launcher")]
    public class MyScoreActivity : Activity
    {
        private int maxScore = 0;
        private int mediumScore = 0;
        private int lastScore = 0;
        private TextView txtBestScore;
        private TextView txtMediumScore;
        private TextView txtMemoryDescription;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.MyScores);

            // READ DATA
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = System.IO.Path.Combine(path, "myfile.txt");
            path = filename;

            /**
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("1");
                sw.WriteLine("2");
                sw.WriteLine("3");
                sw.WriteLine("2");
            }
           */

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    List<int> scoresList = new List<int>();
                    int countValid = 0;
                    int lines = 0;
                    int sumScore = 0;
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        lines++;
                        int actualScore = Convert.ToInt32(s);
                        // lastScore is used to get the last line from the file
                        // should be the last entry and that is the players score
                        lastScore = actualScore;

                        scoresList.Add(actualScore);
                        //sumScore += actualScore;

                        if (actualScore > maxScore)
                        {
                            
                            maxScore = actualScore;
                        }
                    }
                    if (lines > 0)
                    {
                        scoresList.ForEach(delegate (int i)
                        {
                            if (i >= maxScore * 2 / 10)
                            {
                                sumScore += i;
                                countValid++;
                            }
                        });
                        mediumScore = sumScore / countValid;
                    }
                }
            }


            txtBestScore = (TextView)FindViewById<TextView>(Resource.Id.txtBestScore);
            txtMediumScore = FindViewById<TextView>(Resource.Id.txtMediumScore);
            txtMemoryDescription = FindViewById<TextView>(Resource.Id.txtMemoryDescription);

            //String bestScoreText = "<![CDATA[<p><font color='#CBCBCB'>BEST SCORE</font> <font color='#FFFFFF'>3211</font></p> ]]>";
            //txtBestScore.SetText((Html.FromHtml((bestScoreText))));

            var bestScoreString = "BEST SCORE " + maxScore;
            var spanBestScore = new SpannableString(bestScoreString);

            spanBestScore.SetSpan(new ForegroundColorSpan(Color.Rgb(203, 203, 203)), 0, 10, 0);
            spanBestScore.SetSpan(new ForegroundColorSpan(Color.Rgb(255, 255, 255)), 11, bestScoreString.Length, 0);
            spanBestScore.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, 10, 0);
            spanBestScore.SetSpan(new StyleSpan(TypefaceStyle.Bold), 11, bestScoreString.Length, 0);
            txtBestScore.SetText(spanBestScore, TextView.BufferType.Spannable);

            var mediumScoreString = "MEDIUM SCORE " + mediumScore;
            var spanMediumScore = new SpannableString(mediumScoreString);

            spanMediumScore.SetSpan(new ForegroundColorSpan(Color.Rgb(203, 203, 203)), 0, 12, 0);
            spanMediumScore.SetSpan(new ForegroundColorSpan(Color.Rgb(255, 255, 255)), 13, mediumScoreString.Length, 0);
            spanMediumScore.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, 12, 0);
            spanMediumScore.SetSpan(new StyleSpan(TypefaceStyle.Bold), 13, mediumScoreString.Length, 0);
            txtMediumScore.SetText(spanMediumScore, TextView.BufferType.Spannable);

            if (lastScore == 0)
            {
                txtMemoryDescription.Text = "Your score is " + lastScore + ". You can do better! Wait! Are you even human!?";
                return;
            }
            if (lastScore > 0 && lastScore <= mediumScore / 2)
            {
                txtMemoryDescription.Text = "Your score is " + lastScore + ". Your memory is decent, yet we strongly suggest that you improve it.";
                return;
            }
            if (lastScore > mediumScore / 2 && lastScore <= mediumScore)
            {
                txtMemoryDescription.Text = "Your score is " + lastScore + ". You are a little below average, keep working on your memory and you will be a master in no time.";
                return;
            }
            if (lastScore > mediumScore && lastScore <= maxScore / 2)
            {
                txtMemoryDescription.Text = "Your score is " + lastScore + ". You are above average, keep working on your memory and you will be a master in no time.";
                return;
            }
            if (lastScore > maxScore / 2 && lastScore <= maxScore)
            {
                txtMemoryDescription.Text = "Your score is " + lastScore + ". You are a genius! There is always room for improvement! We recommend you to keep practicing. ";
                return;
            }
            // txtMemoryDescription.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc sit amet purus pellentesque, vulputate tellus vitae, posuere nisl. Vivamus volutpat facilisis feugiat. Aliquam id dignissim purus. Cras ut iaculis tortor, ac dignissim diam. Aliquam varius, eros mattis placerat consequat, elit ligula lacinia mi, quis tincidunt orci enim quis risus. Cras pharetra diam felis, nec semper ipsum placerat nec. Phasellus feugiat at neque ac aliquet. Sed hendrerit, tellus a ultricies iaculis, orci odio ornare arcu, molestie lacinia risus orci non magna. Mauris auctor ut ante eu porttitor. Ut nunc elit, vehicula a tempus quis, commodo vel quam. Sed scelerisque, dolor ut tempus auctor, lorem massa suscipit dolor, accumsan tincidunt lectus metus nec massa.";
        }
    }
}