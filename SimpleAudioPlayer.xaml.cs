using NAudio.Wave;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace IELTSAppProject
{
    public partial class SimpleAudioPlayer : UserControl
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;
        private DispatcherTimer timer;
        private bool isPaused = false;

        public string AudioPath { get; set; }

        public SimpleAudioPlayer()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += UpdateProgress;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut == null)
            {
                PlayAudio();
                PlayButton.Content = "| |";
            }
            else if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                PauseAudio();
                PlayButton.Content = "▶";
            }
            else if (isPaused)
            {
                ResumeAudio();
                PlayButton.Content = "| |";
            }
            else
            {
                PlayAudio();
                PlayButton.Content = "| |";
            }
        }

        private void PlayAudio()
        {
            try
            {
                if (string.IsNullOrEmpty(AudioPath)) return;

                audioFile = new AudioFileReader(AudioPath);
                waveOut = new WaveOutEvent();
                waveOut.PlaybackStopped += (s, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (!isPaused)
                        {
                            PlayButton.Content = "▶";
                            ProgressSlider.Value = 0;
                        }
                    });
                };

                waveOut.Init(audioFile);
                waveOut.Play();
                timer.Start();
                isPaused = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении аудио: {ex.Message}");
                StopAudio();
            }
        }

        private void PauseAudio()
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                timer.Stop();
                isPaused = true;
            }
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

        private void ResumeAudio()
        {
            if (waveOut != null && isPaused)
            {
                waveOut.Play();
                timer.Start();
                isPaused = false;
            }
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