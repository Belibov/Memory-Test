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
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace Memory_Test
{
    [Activity(Label = "TestActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/ic_launcher")]
    public class TestActivity : Activity
    {
        private int level;
        private int combo;
        private int lives;
        private int score;

        private TextView txtTestLevel;
        private TextView txtYourLives;
        private TextView txtYourCombo;
        private TextView txtTestScore;

        private Button[] testButtons = new Button[9];
        private int[] pressedIndexes = new int[9];

        //private List<int> automatPressed = new List<int>();
        private int btn_ind;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Test);

            lives = 3;
            combo = 0;
            level = 1;
            score = 0;
            txtTestLevel = FindViewById<TextView>(Resource.Id.txtTestLevel);
            txtYourLives = FindViewById<TextView>(Resource.Id.txtYourLives);
            txtYourCombo = FindViewById<TextView>(Resource.Id.txtYourCombo);
            txtTestScore = FindViewById<TextView>(Resource.Id.txtTestScore);

            this.assignAllButtons();
            this.setAllInfo();
            this.start();
            for (int i = 0; i < 9; i++)
            {
                var button = testButtons[i];
                button.Tag = i;
                button.Click += testButtons_Click;
            }
        }

        private void testButtons_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var index = (int)button.Tag;
                System.Diagnostics.Debug.WriteLine("INDEX OF CLICKED BUTTON: " + index);
                if (index == pressedIndexes[btn_ind] && btn_ind == this.getCountButtons() - 1)
                {
                    this.onSuccessSolve();
                }
                else if (index == pressedIndexes[btn_ind])
                {
                    button.SetBackgroundResource(Resource.Drawable.TestButtonChecked);
                    button.Click -= testButtons_Click;
                }
                else
                {
                    this.onFailSolve();
                }
            }
            btn_ind++;
        }

        private void start()
        {
            setAllInfo();
            resetButtons();
            resetPressedIndexes();
            generateRandomButtons();
            activateButtonsDemo();
        }

        private void setAllInfo()
        {
            this.setCurrentLevel();
            this.setCurrentLives();
            this.setCurrentCombo();
            this.setCurrentScore();
        }

        private double getWaitTime()
        {
            if (this.level < 5)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 8;
            }
            else if (this.level < 10)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 7;
            }
            else if (this.level < 15)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 6;
            }
            else if (this.level < 20)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 5;
            }
            else if (this.level < 25)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 4.5;
            }
            else if (this.level < 30)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 4;
            }
            else if (this.level < 40)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 3.2;
            }
            else if (this.level < 50)
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 2.4;
            }
            else
            {
                return Math.Sqrt(10.0 + this.level) / (10.0 + this.level) * 1.2;
            }
        }

        private void onSuccessSolve()
        {
            this.score += (int)((1 + this.level * 0.5) * 10 * (1 + this.combo * 0.1));
            this.combo++;
            this.level++;
            this.resetPressedIndexes();
            this.setAllInfo();
            this.resetButtons();
            this.start();
        }

        private void onFailSolve()
        {
            this.lives--;
            this.combo = 0;
            if (this.lives <= 0)
            {
                this.endTest();
                return;
            }
            this.level++;
            this.resetPressedIndexes();
            this.setAllInfo();
            this.resetButtons();
        }

        async private void endTest()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = System.IO.Path.Combine(path, "myfile.txt");
            path = filename;
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(score);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(score);
                }
            }
            txtTestScore.Text = "Test finished!";
            await Task.Delay(4000);
            lives = 3;
            combo = 0;
            level = 1;
            score = 0;
            this.resetButtons();
            this.resetPressedIndexes();
            this.setAllInfo();
        }

        async private void activateButtonsDemo()
        {
            int count = this.getCountButtons();
            double time = this.getWaitTime();
            int mseconds = (int)(time * 1000);

            for (int i = 0; i < count; i++)
            {
                this.testButtons[i].SetBackgroundResource(Resource.Drawable.TestButtonChecked);
                System.Threading.Thread.Sleep(mseconds);
            }
            for (int i = 0; i < 9; i++)
            {
                testButtons[i].Click -= testButtons_Click;
            }
            /*int count = this.getCountButtons();
            double time = this.getWaitTime();
            int mseconds = (int)(time * 1000);*/

            for (int i = 0; i < count; i++)
            {
                System.Diagnostics.Debug.WriteLine("Activating button " + i);
                testButtons[pressedIndexes[i]].SetBackgroundResource(Resource.Drawable.TestButtonChecked);
                await Task.Delay(mseconds);
            }
            //resetButtons();

            this.resetButtons();
            for (int i = 0; i < 9; i++)
            {
                testButtons[i].Click += testButtons_Click;
            }
        }
        
        private void generateRandomButtons()
        {
            int t = -1;
            Random random = new Random();
            for (int i = 0; i < this.getCountButtons(); i++)
            {
                t = random.Next(0, 9);
                while (this.pressedIndexes.Contains(t))
                {
                    t = random.Next(0, 9);
                }
                System.Diagnostics.Debug.WriteLine("GENERATED INDEX button " + t);
                this.pressedIndexes[i] = t;
            }
        }

        private void resetButtons()
        {
            for (int i = 0; i < 9; i++)
            {
                this.testButtons[i].SetBackgroundResource(Resource.Drawable.TestButtonUnchecked);
            }
            btn_ind = 0;
        }

        private void assignAllButtons()
        {
            this.testButtons[0] = FindViewById<Button>(Resource.Id.testThree1);
            this.testButtons[1] = FindViewById<Button>(Resource.Id.testThree2);
            this.testButtons[2] = FindViewById<Button>(Resource.Id.testThree3);
            this.testButtons[3] = FindViewById<Button>(Resource.Id.testThree4);
            this.testButtons[4] = FindViewById<Button>(Resource.Id.testThree5);
            this.testButtons[5] = FindViewById<Button>(Resource.Id.testThree6);
            this.testButtons[6] = FindViewById<Button>(Resource.Id.testThree7);
            this.testButtons[7] = FindViewById<Button>(Resource.Id.testThree8);
            this.testButtons[8] = FindViewById<Button>(Resource.Id.testThree9);
        }

        private void resetPressedIndexes()
        {
            for (int i = 0; i < 9; i++)
            {
                pressedIndexes[i] = -1;
            }
        }

        private int getCountButtons()
        {
            if (this.level < 5)
            {
                return 1;
            }
            else if (this.level < 9)
            {
                return 2;
            }
            else if (this.level < 17)
            {
                return 3;
            }
            else if (this.level < 25)
            {
                return 4;
            }
            else if (this.level < 35)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        private void setCurrentLevel()
        {
            var currentString = "LEVEL " + level;
            var spanCurrentString = new SpannableString(currentString);

            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(203, 203, 203)), 0, 5, 0);
            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(255, 255, 255)), 6, currentString.Length, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, 5, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Bold), 6, currentString.Length, 0);
            txtTestLevel.SetText(spanCurrentString, TextView.BufferType.Spannable);
        }

        private void setCurrentLives()
        {
            var currentString = "LIVES " + lives;
            var spanCurrentString = new SpannableString(currentString);

            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(203, 203, 203)), 0, 5, 0);
            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(255, 255, 255)), 6, currentString.Length, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, 5, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Bold), 6, currentString.Length, 0);
            txtYourLives.SetText(spanCurrentString, TextView.BufferType.Spannable);
        }

        private void setCurrentCombo()
        {
            var currentString = "COMBO X" + combo;
            var spanCurrentString = new SpannableString(currentString);

            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(203, 203, 203)), 0, 5, 0);
            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(255, 255, 255)), 6, currentString.Length, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, 5, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Bold), 6, currentString.Length, 0);
            txtYourCombo.SetText(spanCurrentString, TextView.BufferType.Spannable);
        }

        private void setCurrentScore()
        {
            var currentString = score + " POINTS";
            var spanCurrentString = new SpannableString(currentString);

            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(203, 203, 203)), currentString.Length - 6, currentString.Length, 0);
            spanCurrentString.SetSpan(new ForegroundColorSpan(Color.Rgb(255, 255, 255)), 0, currentString.Length - 7, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Normal), currentString.Length - 6, currentString.Length, 0);
            spanCurrentString.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, currentString.Length - 7, 0);
            txtTestScore.SetText(spanCurrentString, TextView.BufferType.Spannable);
        }
    }
}