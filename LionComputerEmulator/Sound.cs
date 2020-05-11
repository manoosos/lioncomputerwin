using System;
using NAudio.Wave;
using System.Threading;
using System.ComponentModel;

namespace LionComputerEmulator
{
    /// <summary>
    /// Sound Class provides sound.
    /// </summary>
    public static class Sound
    {
        private static WaveOut waveOutCh1;
        private static WaveOut waveOutCh2;

        private static LionWaveProvider32 lionWaveProvider1 = new LionWaveProvider32();
        private static LionWaveProvider32 lionWaveProvider2 = new LionWaveProvider32();

        private static bool soundTrigger1 = false;
        private static bool soundTrigger2 = false;

        public static BackgroundWorker WorkerChannel1 = new BackgroundWorker();
        public static BackgroundWorker WorkerChannel2 = new BackgroundWorker();

        private static void onLionWave1EndSound(object sender, EventArgs e)
        {
            lock (copyLock1)
                Device.Port[Device.SOUND_STATUS] &= (ushort)0x0fffe;    // channel1 stop
        }

        private static void onLionWave2EndSound(object sender, EventArgs e)
        {
            lock (copyLock2)
                Device.Port[Device.SOUND_STATUS] &= (ushort)0x0fffd;    // channel2 stop
        }

        private static void doWork1(object sender, DoWorkEventArgs e)
        {
            lock (copyLock1)
                soundTrigger1 = true;
        }

        private static void doWork2(object sender, DoWorkEventArgs e)
        {
            lock (copyLock2)
                soundTrigger2 = true;
        }

        public static void Init()
        {
            WorkerChannel1.DoWork += doWork1;
            WorkerChannel2.DoWork += doWork2;

            lionWaveProvider1.SoundEnd += new EventHandler(onLionWave1EndSound);
            lionWaveProvider2.SoundEnd += new EventHandler(onLionWave2EndSound);

            lionWaveProvider1.SetWaveFormat(22050, 1);
            lionWaveProvider1.Amplitude = 0.25f;
            waveOutCh1 = new WaveOut();
            waveOutCh1.Init(lionWaveProvider1);
            waveOutCh1.Play();

            lionWaveProvider2.SetWaveFormat(22050, 1);
            lionWaveProvider2.Amplitude = 0.25f;
            waveOutCh2 = new WaveOut();
            waveOutCh2.Init(lionWaveProvider2);
            waveOutCh2.Play();
        }

        /// <summary>
        /// Thread a Beep to Channel1
        /// </summary>
        public static void PlayBeep1()
        {
            int portValue;
            int frequency;
            int msDuration;
            while (Cpu.isRunning)
            {
                if (soundTrigger1)
                {
                    portValue = 0;
                    soundTrigger1 = false;
                    lock (copyLock1)
                        portValue = Device.Port[Device.SOUND_CONTROL];
                    frequency = (portValue & 0x03fff);
                    if (frequency > 0)
                    {
                        frequency = 100000 / frequency;
                        msDuration = (portValue & 0x0c000);

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

                        lionWaveProvider1.Frequency = frequency;
                        lionWaveProvider1.MsDuration = msDuration;
                        lionWaveProvider1.NoiseFlag = Device.Port[Device.SOUND_NOISE_FLAG] == 1;
                        lionWaveProvider1.StartSound();
                    }
                }
                Thread.Sleep(0);
            }
        }

        public static void PlayBeep2()
        {
            int portValue;
            int frequency;
            int msDuration;
            while (Cpu.isRunning)
            {
                if (soundTrigger2)
                {
                    portValue = 0;
                    soundTrigger2 = false;
                    lock (copyLock2)
                        portValue = Device.Port[Device.SOUND_CONTROL_2];
                    frequency = (portValue & 0x03fff);
                    if (frequency > 0)
                    {
                        frequency = 100000 / frequency;
                        msDuration = (portValue & 0x0c000);

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

                        lionWaveProvider2.Frequency = frequency;
                        lionWaveProvider2.MsDuration = msDuration;
                        lionWaveProvider2.NoiseFlag = Device.Port[Device.SOUND_NOISE_FLAG] == 1;
                        lionWaveProvider2.StartSound();
                    }
                }
                Thread.Sleep(0);
            }
        }

        public static object copyLock1 = new object();
        public static object copyLock2 = new object();
    }
}
