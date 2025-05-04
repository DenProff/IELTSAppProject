using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using DocumentFormat.OpenXml.Bibliography;
using NAudio.Wave;
using System.IO;
using System.IO.Packaging;

namespace IELTSAppProject
{
    public abstract class SoundControl
    {
        private static WaveInEvent waveIn; // Для записи звука с микрофона
        private static WaveFileWriter writer; // Для записи в wav
        private static WaveOutEvent waveOut; // Для вывода звука
        private static AudioFileReader audioFile; // Для считывания аудиофайла
        private static string audioPath; // Путь к аудиофайлу
        private static bool isPaused = false; // Флаг для приостановки
        private static string outputFilePath = "recordedForTest.wav"; // Куда идёт запись - именно файл расширения .wav

        public static string AudioPath { get => audioPath; set => audioPath = value; }
        public static bool IsPaused { get => isPaused; set => isPaused = value; }
        public static WaveOutEvent WaveOut { get => waveOut; set => waveOut = value; }
        public static AudioFileReader AudioFile { get => audioFile; set => audioFile = value; }

        public static void StartRecording() // Начать запись
        {
            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0; // 0 это микрофон по умолчанию
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1); // Параметры записи
            writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);

            waveIn.DataAvailable += (s, e) =>
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            waveIn.RecordingStopped += (s, e) =>
            {
                writer?.Dispose();
                waveIn?.Dispose();
                waveIn = null;
            };
            waveIn.StartRecording();
        }

        public static void StopRecording() // Закончить запись
        {
            waveIn?.StopRecording();
        }

        public static void PlayAudio()
        {
            try
            {
                if (string.IsNullOrEmpty(AudioPath)) return;

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\.."));
                AudioPath = Path.Combine(projectRoot, "audio", AudioPath);
                AudioFile = new AudioFileReader(AudioPath);
                WaveOut = new WaveOutEvent();

                WaveOut.Init(AudioFile);
                WaveOut.Play();
                IsPaused = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении аудио: {ex.Message}");
                StopAudio();
            }
        }

        public static void PauseAudio()
        {
            if (WaveOut != null && WaveOut.PlaybackState == PlaybackState.Playing)
            {
                WaveOut.Pause();
                IsPaused = true;
            }
        }

        public static void StopAudio()
        {
            WaveOut?.Stop();
            AudioFile?.Dispose();
            WaveOut?.Dispose();
            WaveOut = null;
            AudioFile = null;
        }

        public static void ResumeAudio()
        {
            if (WaveOut != null && IsPaused)
            {
                WaveOut.Play();
                IsPaused = false;
            }
        }
    }
}
