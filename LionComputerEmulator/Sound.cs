using System;
using System.IO;
using System.Media;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LionComputerEmulator
{
    /// <summary>
    /// Sound Class provides sound.
    /// </summary>
    public static class Sound
    {
        private static NAudio.Wave.WaveOutEvent WaveEventCh1 = new NAudio.Wave.WaveOutEvent();
        private static NAudio.Wave.WaveOutEvent WaveEventCh2 = new NAudio.Wave.WaveOutEvent();

        private static bool soundTrigger1 = false;
        private static bool soundTrigger2 = false;

        public static BackgroundWorker WorkerChannel1 = new BackgroundWorker();
        public static BackgroundWorker WorkerChannel2 = new BackgroundWorker();

        private static void doWork1(object sender, DoWorkEventArgs e)
        {
            lock (copyLock1)
                soundTrigger1 = true;// Convert.ToBoolean(e.Argument);
        }

        private static void doWork2(object sender, DoWorkEventArgs e)
        {
            lock (copyLock2)
                soundTrigger2 = true;// Convert.ToBoolean(e.Argument);
        }

        public static void Init()
        {
            //WaveEventCh1.PlaybackStopped += PlaybackStoppedCh1;
            //WaveEventCh2.PlaybackStopped += PlaybackStoppedCh2;
            WorkerChannel1.DoWork += doWork1;
            WorkerChannel2.DoWork += doWork2;
        }

        /// <summary>
        /// Thread a Beep to Channel1
        /// </summary>
        public static void PlayBeep1()
        {
            while (Cpu.isRunning)
            {
                if (soundTrigger1)
                {
                    int portValue = 0;
                    WaveEventCh1.Stop();
                    soundTrigger1 = false;
                    lock (copyLock1)
                        portValue = Device.Port[Device.SOUND_CONTROL];
                    int frequency = (portValue & 0x03fff);
                    if (frequency > 0)
                    {
                        frequency = 100000 / frequency;
                        int msDuration = (portValue & 0x0c000);
                        int amplitude = 16380; // Max amplitude for 16-bit audio

                        switch (msDuration)
                        {
                            case 0x04000:
                                msDuration = 500;
                                break;

                            case 0x08000:
                                msDuration = 250;
                                break;

                            case 0x0c000:
                                msDuration = 125;
                                break;

                            default:
                                msDuration = 1000;
                                break;
                        }

                        using (MemoryStream mStrm = new MemoryStream())
                        {
                            using (BinaryWriter writer = new BinaryWriter(mStrm))
                            {
                                int formatChunkSize = 16;
                                int headerSize = 8;
                                short formatType = 1; // 1 (MS PCM)
                                short tracks = 2;
                                int samplesPerSecond = 44100;// frequency * 2;
                                short bitsPerSample = 16;
                                short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                                int bytesPerSecond = samplesPerSecond * frameSize;
                                int waveSize = 4;
                                int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
                                int dataChunkSize = samples * frameSize;
                                int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
                                writer.Write(0x46464952); // "RIFF"
                                writer.Write(fileSize);
                                writer.Write(0x45564157); // "WAVE"
                                writer.Write(0x20746D66); // "fmt "
                                writer.Write(formatChunkSize);
                                writer.Write(formatType);
                                writer.Write(tracks);
                                writer.Write(samplesPerSecond);
                                writer.Write(bytesPerSecond);
                                writer.Write(frameSize);
                                writer.Write(bitsPerSample);
                                writer.Write(0x61746164); // "data"
                                writer.Write(dataChunkSize);
                                {
                                    double t = (Math.PI * 2 * frequency) / (samplesPerSecond * tracks);
                                    Random rnd = new Random();

                                    // generate square wave
                                    for (int step = 0; step < samples - 1; step++)
                                    {
                                        short toneSample = Convert.ToInt16(amplitude * Math.Sign(Math.Sin(t * step)));
                                        writer.Write(toneSample);
                                        // if noise flag then noise to 2nd track else tone
                                        writer.Write(Device.Port[Device.SOUND_NOISE_FLAG] == 1 ?
                                            Convert.ToInt16(rnd.Next(2) == 0 ? -amplitude : amplitude) :
                                            toneSample);
                                    }
                                }
                                writer.Flush();
                                mStrm.Seek(0, SeekOrigin.Begin);
                                WaveEventCh1.Init(new NAudio.Wave.WaveFileReader(mStrm));
                                WaveEventCh1.Play();
                                while (WaveEventCh1.PlaybackState == PlaybackState.Playing && !soundTrigger1)
                                    System.Threading.Thread.Sleep(1);
                            }
                        }
                    }
                    Device.Port[Device.SOUND_STATUS] &= (ushort)0x0fffe;    // channel stop
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }

        //private static void PlaybackStoppedCh1(object sender, NAudio.Wave.StoppedEventArgs e)
        //{
        //    //WaveEventCh1.Dispose();
        //    lock (copyLock1)
        //        Device.Port[Device.SOUND_STATUS] &= (ushort)0x0fffe;    // channel stop
        //    //System.Threading.Thread.Sleep(1);
        //}

        //private static void PlaybackStoppedCh2(object sender, NAudio.Wave.StoppedEventArgs e)
        //{
        //    //WaveEventCh2.Dispose();
        //    lock (copyLock2)
        //        Device.Port[Device.SOUND_STATUS] &= (ushort)0x0fffd;    // channel stop
        //    //System.Threading.Thread.Sleep(1);
        //}

        /// <summary>
        /// Thread a Beep to Channel2
        /// </summary>
        public static void PlayBeep2()
        {
            while (Cpu.isRunning)
            {
                if (soundTrigger2)
                {
                    int portValue = 0;
                    WaveEventCh2.Stop();
                    soundTrigger2 = false;
                    lock (copyLock2)
                        portValue = Device.Port[Device.SOUND_CONTROL_2];
                    int frequency = (portValue & 0x03fff);
                    if (frequency > 0)
                    {
                        frequency = 100000 / frequency;
                        int msDuration = (portValue & 0x0c000);
                        int amplitude = 16380;  // Max amplitude for 16-bit audio

                        switch (msDuration)
                        {
                            case 0x04000:
                                msDuration = 500;
                                break;

                            case 0x08000:
                                msDuration = 250;
                                break;

                            case 0x0c000:
                                msDuration = 125;
                                break;

                            default:
                                msDuration = 1000;
                                break;
                        }

                        using (MemoryStream mStrm = new MemoryStream())
                        {
                            using (BinaryWriter writer = new BinaryWriter(mStrm))
                            {
                                int formatChunkSize = 16;
                                int headerSize = 8;
                                short formatType = 1; // 1 (MS PCM)
                                short tracks = 2;
                                int samplesPerSecond = 44100;// frequency * 2;
                                short bitsPerSample = 16;
                                short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                                int bytesPerSecond = samplesPerSecond * frameSize;
                                int waveSize = 4;
                                int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
                                int dataChunkSize = samples * frameSize;
                                int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
                                writer.Write(0x46464952); // "RIFF"
                                writer.Write(fileSize);
                                writer.Write(0x45564157); // "WAVE"
                                writer.Write(0x20746D66); // "fmt "
                                writer.Write(formatChunkSize);
                                writer.Write(formatType);
                                writer.Write(tracks);
                                writer.Write(samplesPerSecond);
                                writer.Write(bytesPerSecond);
                                writer.Write(frameSize);
                                writer.Write(bitsPerSample);
                                writer.Write(0x61746164); // "data"
                                writer.Write(dataChunkSize);
                                {
                                    double t = (Math.PI * 2 * frequency) / (samplesPerSecond * tracks);
                                    Random rnd = new Random();

                                    // generate square wave
                                    for (int step = 0; step < samples - 1; step++)
                                    {
                                        short toneSample = Convert.ToInt16(amplitude * Math.Sign(Math.Sin(t * step)));
                                        writer.Write(toneSample);
                                        // if noise flag then noise to 2nd track else tone
                                        writer.Write(Device.Port[Device.SOUND_NOISE_FLAG] == 1 ?
                                            Convert.ToInt16(rnd.Next(2) == 0 ? -amplitude : amplitude) :
                                            toneSample);
                                    }
                                }
                                writer.Flush();
                                mStrm.Seek(0, SeekOrigin.Begin);
                                WaveEventCh2.Init(new NAudio.Wave.WaveFileReader(mStrm));
                                WaveEventCh2.Play();
                                while (WaveEventCh2.PlaybackState == PlaybackState.Playing && !soundTrigger2)
                                    System.Threading.Thread.Sleep(1);
                            }
                        }
                    }
                    Thread.Sleep(1);
                    Device.Port[Device.SOUND_STATUS] &= (ushort)0x0fffd;    // channel stop
                }
                Thread.Sleep(1);
            }
        }

        public static object copyLock1 = new object();
        public static object copyLock2 = new object();
    }
}
