using NAudio.Wave;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace IELTSAppProject
{
    public partial class SimpleAudioPlayer : UserControl
    {
        private DispatcherTimer timer;
        public static readonly DependencyProperty AudioPathProperty =
        DependencyProperty.Register("AudioPath", typeof(string), typeof(SimpleAudioPlayer));

        public string AudioPath
        {
            get => (string)GetValue(AudioPathProperty);
            set => SetValue(AudioPathProperty, value);
        }

        public SimpleAudioPlayer()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += UpdateProgress;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            SoundControl.AudioPath = AudioPath;
            if (SoundControl.WaveOut == null)
            {
                SoundControl.PlayAudio();
                timer.Start();
            }
            else if (SoundControl.WaveOut.PlaybackState == PlaybackState.Playing)
            {
                SoundControl.PauseAudio();
                timer.Stop();
            }
            else if (SoundControl.IsPaused)
            {
                SoundControl.ResumeAudio();
                timer.Start();
            }
            else
            {
                SoundControl.PlayAudio();
            }
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            if (SoundControl.AudioFile != null)
            {
                ProgressSlider.Value = SoundControl.AudioFile.CurrentTime.TotalSeconds / SoundControl.AudioFile.TotalTime.TotalSeconds * 100;
                TimeText.Text = SoundControl.AudioFile.CurrentTime.ToString(@"m\:ss") + " / " + SoundControl.AudioFile.TotalTime.ToString(@"m\:ss");
            }
        }
    }
}