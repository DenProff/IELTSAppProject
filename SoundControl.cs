using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using NAudio.Wave;

namespace IELTSAppProject
{
    public abstract class SoundControl
    {
        static WaveInEvent waveIn; // Для записи звука с микрофона
        static WaveFileWriter writer; // Для записи в wav
        static string outputFilePath = "recordedForTest.wav"; // Куда идёт запись - именно файл расширения .wav

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
    }
}
