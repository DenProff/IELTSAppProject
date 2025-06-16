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
        private static SimpleAudioPlayer currentlyPlayingInstance;

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

            // Остановить воспроизведение при уничтожении контрола
            this.Unloaded += (s, e) => StopPlayback();
            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Если другой экземпляр уже играет, то он останавливается
            if (currentlyPlayingInstance != null && currentlyPlayingInstance != this)
            {
                currentlyPlayingInstance.StopPlayback();
            }

            // Текущий экземпляр устанавливается как активный
            currentlyPlayingInstance = this;

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
                timer.Start();
            }
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            if (SoundControl.AudioFile != null && SoundControl.WaveOut != null &&
                SoundControl.WaveOut.PlaybackState == PlaybackState.Playing)
            {
                ProgressSlider.Value = SoundControl.AudioFile.CurrentTime.TotalSeconds /
                                    SoundControl.AudioFile.TotalTime.TotalSeconds * 100;
                TimeText.Text = SoundControl.AudioFile.CurrentTime.ToString(@"m\:ss") + " / " +
                                SoundControl.AudioFile.TotalTime.ToString(@"m\:ss");
            }
        }

        private void StopPlayback()
        {
            if (SoundControl.WaveOut != null)
            {
                SoundControl.StopAudio();
                timer.Stop();

                if (currentlyPlayingInstance == this)
                {
                    currentlyPlayingInstance = null;
                }
            }
        }
        public static string[] resourcesKeysArray =
        {
        "playOrPause"
        }; // Массив с ключами для ресурсов - необходимо для реализации многоязычности
        
    }
}