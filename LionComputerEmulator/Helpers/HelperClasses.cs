using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace LionComputerEmulator
{
    //--------------------------------------------------
    // disassembler symbols
    //--------------------------------------------------
    public class DasmSymbol
    {
        public string Name;
        public string HexValue;
        public string BinaryValue;
        public uint DecimalValue;
        public bool isLabel;
    }

    public class DasmRecord
    {
        public List<DasmSymbol> SymbolsList = new List<DasmSymbol>();
    }

    //--------------------------------------------------
    // Lion Sound NAudio wave
    //--------------------------------------------------

    public abstract class WaveProvider32 : IWaveProvider
    {
        private WaveFormat waveFormat;

        public WaveProvider32() : this(44100, 1)
        {
        }

        public WaveProvider32(int sampleRate, int channels)
        {
            SetWaveFormat(sampleRate, channels);
        }

        protected int SampleRate;
        protected int Tracks;

        public void SetWaveFormat(int sampleRate, int channels)
        {
            SampleRate = sampleRate;
            Tracks = channels;
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            WaveBuffer waveBuffer = new WaveBuffer(buffer);
            int samplesRequired = count / 4;
            int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
            return samplesRead * 4;
        }

        public abstract int Read(float[] buffer, int offset, int sampleCount);

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }
    }

    class LionWaveProvider32 : WaveProvider32
    {
        public int MsDuration = 0;
        public bool NoiseFlag = false;
        public float Frequency = 0;
        public float Amplitude { get; set; }

        public EventHandler SoundEnd;

        int sample;
        int samplecnt = 0;
        float samplevalue, noisevalue;
        float t;
        int n;
        Random random;

        public void StartSound()
        {
            samplecnt = (int)((MsDuration * .001) / (1.0 / SampleRate));
            t = (float)(Math.PI * 2.0 * Frequency) / (SampleRate * Tracks);
            random = new Random();
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (n = 0; n < sampleCount; n++)
            {
                samplevalue = 0;
                if (samplecnt-- > 0 && Frequency != 0)
                {
                    samplevalue = (float)(Amplitude * Math.Sign(Math.Sin(t * sample)));
                    if (NoiseFlag)
                    {
                        noisevalue = random.Next(2) == 0 ? Amplitude : -Amplitude;
                        samplevalue = (samplevalue + noisevalue) / 2;
                    }
                }
                buffer[n + offset] = samplevalue;

                if (sample++ >= SampleRate)
                    sample = 0;

                if (samplecnt == 0)
                    SoundEnd?.Invoke(this, null);
                else if (samplecnt < 0)
                    samplecnt = 0;
            }
            return sampleCount;
        }
    }
}
