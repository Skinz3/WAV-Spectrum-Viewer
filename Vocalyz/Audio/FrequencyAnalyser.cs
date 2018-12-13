using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vocalyz.Audio
{
    public class FrequencyAnalyser
    {
        public const double FFT_LENGHT_44100_FACTOR = 1.345565749235474;

        public int FFTLength
        {
            get;
            private set;
        }
        public int SampleRate
        {
            get
            {
                return m_playback.GetInputStream().WaveFormat.SampleRate;
            }
        }
        private AudioPlayback m_playback;
        public event EventHandler<AnalyserEventArgs> Calculated;


        public FrequencyAnalyser(string filePath, int fftLength = 1024)
        {
            m_playback = new AudioPlayback();
            m_playback.FftCalculated += OnFFTCalculated;
            m_playback.Load(filePath, fftLength);
            FFTLength = fftLength;


            //  var numberOfSamples = msToSamples((int)m_playback.GetInputStream().TotalTime.TotalMilliseconds, 44100, m_playback.GetInputStream().WaveFormat.Channels);

        }

        /*   int msToSamples(int ms, int sampleRate, int channels)
           {
               return (int)(((long)ms) * sampleRate * channels / 1000);
           }*/

        private void OnFFTCalculated(object sender, FftEventArgs e)
        {
            FFTResult[] result = new FFTResult[e.Result.Length];


            for (int i = 1; i < e.Result.Length + 1; i++)
            {

                Complex c = e.Result[i - 1];
                int frequency = (i - 1) * SampleRate / FFTLength;
                double intensity = Math.Sqrt(c.X * c.X + c.Y * c.Y);

                result[i - 1] = new FFTResult((int)(frequency), intensity); // frequency * * FFT_LENGHT_44100_FACTOR for 44100 fft size
            }


            Calculated?.Invoke(sender, new AnalyserEventArgs(new SampleAnalysis(result)));

           
        }

        public void Pause()
        {
            m_playback.Pause();
        }

        public void Play()
        {
            m_playback.Play();
        }

        public void Dispose()
        {
            m_playback.Dispose();
        }

        public class AnalyserEventArgs : EventArgs
        {
            [DebuggerStepThrough]
            public AnalyserEventArgs(SampleAnalysis result)
            {
                Result = result;
            }
            public SampleAnalysis Result { get; private set; }
        }
    }
}
