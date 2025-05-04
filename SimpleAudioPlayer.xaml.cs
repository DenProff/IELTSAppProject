using NAudio.Wave;
using System;
using System.Windows;
using System.Windows.Controls;

namespace IELTSAppProject
{
    public partial class SimpleAudioPlayer : UserControl
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;
        private System.Windows.Threading.DispatcherTimer timer;

        public static readonly DependencyProperty AudioPathProperty =
            DependencyProperty.Register("AudioPath", typeof(string), typeof(SimpleAudioPlayer),
                new PropertyMetadata(null));

        public string AudioPath
        {
            get => (string)GetValue(AudioPathProperty);
            set => SetValue(AudioPathProperty, value);
        }

        public SimpleAudioPlayer()
        {
            InitializeComponent();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += UpdateProgress;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut == null || waveOut.PlaybackState != PlaybackState.Playing)
            {
                PlayAudio();
                PlayButton.Content = "⏸";
            }
            else
            {
                PauseAudio();
                PlayButton.Content = "▶";
            }
        }

        private void PlayAudio()
        {
            try
            {
                if (string.IsNullOrEmpty(AudioPath)) return;

                StopAudio();

                audioFile = new AudioFileReader(AudioPath);
                waveOut = new WaveOutEvent();
                waveOut.PlaybackStopped += (s, e) => {
                    PlayButton.Content = "▶";
                    ProgressSlider.Value = 0;
                };

                waveOut.Init(audioFile);
                waveOut.Play();
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении аудио: {ex.Message}");
            }
        }

        private void PauseAudio()
        {
            waveOut?.Pause();
            timer.Stop();
        }

        private void StopAudio()
        {
            waveOut?.Stop();
            audioFile?.Dispose();
            waveOut?.Dispose();
            waveOut = null;
            audioFile = null;
            timer.Stop();
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                ProgressSlider.Value = audioFile.CurrentTime.TotalSeconds / audioFile.TotalTime.TotalSeconds * 100;
                TimeText.Text = audioFile.CurrentTime.ToString(@"m\:ss");
            }
        }
    }
}