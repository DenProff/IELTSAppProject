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

        public static bool IsRecordingDeviceAvailable
        {
            get
            {
                try
                {
                    return WaveIn.DeviceCount > 1;
                }
                catch
                {
                    return false;
                }
            }
        }
        private static WaveInEvent waveIn; // Для записи звука с микрофона
        private static WaveFileWriter writer; // Для записи в wav
        private static WaveOutEvent waveOut; // Для вывода звука
        private static AudioFileReader audioFile; // Для считывания аудиофайла
        private static string audioPath; // Путь к аудиофайлу
        private static bool isPaused = false; // Флаг для приостановки

        public static string AudioPath { get => audioPath; set => audioPath = value; }
        public static bool IsPaused { get => isPaused; set => isPaused = value; }
        public static WaveOutEvent WaveOut { get => waveOut; set => waveOut = value; }
        public static AudioFileReader AudioFile { get => audioFile; set => audioFile = value; }

        public static string GetAnswerFilePath(int taskId, bool isUserAudioNeeded) // Метод для получения пути к файлу ответа пользователя или идеальному ответу speaking
        {
            // Получение пути к папке проекта
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\.."));

            // Путь к папке speakingAudioUsersAnswers
            string userAnswersDir = Path.Combine(projectRoot, isUserAudioNeeded ? "speakingAudioUsersAnswers" : "speakingAudioIdealAnswers");

            // Возвращение полного пути к файлу
            return Path.Combine(userAnswersDir, isUserAudioNeeded ? $"userAnswer{taskId}.wav" : $"idealAnswer{taskId}.wav");
        }

        public static void StartRecording(int taskId, bool isUserAudioNeeded) // Название файла для записи ответа формируется на основе id задания speaking
        {
            string filePath = GetAnswerFilePath(taskId, isUserAudioNeeded);
            
            string dir = Path.GetDirectoryName(filePath);

            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            writer = new WaveFileWriter(filePath, waveIn.WaveFormat);

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
                AudioPath = Path.Combine(projectRoot, "listeningAudio", AudioPath);
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

        // Ниже метод перевода Mp3 в массив byte - не нужен но может пригодиться
        //public static byte[] ConvertMp3ToWavBytes(string mp3Path)
        //{
        //    // Временный файл для конвертации
        //    string tempWavPath = Path.GetTempFileName();

        //    try
        //    {
        //        // Конвертация MP3 → WAV (через временный файл)
        //        using (var mp3Reader = new Mp3FileReader(mp3Path))
        //        using (var waveWriter = new WaveFileWriter(tempWavPath, mp3Reader.WaveFormat))
        //        {
        //            mp3Reader.CopyTo(waveWriter);
        //        }

        //        // Чтение WAV в массив байтов
        //        return File.ReadAllBytes(tempWavPath);
        //    }
        //    finally
        //    {
        //        // Удаление временного файла
        //        if (File.Exists(tempWavPath))
        //            File.Delete(tempWavPath);
        //    }
        //}
    }
}
