using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vocalyz.Audio;
using Vocalyz.Music;

namespace Vocalyz.Spectrum.Graphics
{
    public class DrawableSpectrum
    {
        public static int FFT_LENGTH = 4096;//4096;//4096;//44100;//1024;// (int) Math.Pow(2, 16);// 44100;

        public const int WIDTH = 1024;

        public const int HEIGHT = 600;

        public const int MAX_FREQUENCY = 1500;

        private Rectangle Rectangle
        {
            get;
            set;
        }
        private Point Position
        {
            get
            {
                return new Point(SpectrumView.WIDOW_WIDTH / 2 - WIDTH / 2, SpectrumView.WINDOW_HEIGHT - HEIGHT);
            }
        }
        private DrawableLine[] Lines
        {
            get;
            set;
        }
        private FrequencyAnalyser Analyser
        {
            get;
            set;
        }
        public DrawableSpectrum(string filePath)
        {
            this.Rectangle = new Rectangle(Position.X, Position.Y, WIDTH, HEIGHT);
            this.Analyser = new FrequencyAnalyser(filePath, FFT_LENGTH);
            this.Analyser.Calculated += OnAnalyserCalculated;

            int gap = Analyser.SampleRate / Analyser.FFTLength;

            if (gap == 0)
                gap = 1;

            int linesCount = (MAX_FREQUENCY / gap) + 1;

            this.Lines = new DrawableLine[linesCount];

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new DrawableLine(new Vector2(), new Vector2(), Color.White);
            }
        }

        public void Pause()
        {
            Analyser.Pause();
        }

        public void Dispose()
        {
            Analyser.Dispose();
        }
        

      
        private void OnAnalyserCalculated(object sender, FrequencyAnalyser.AnalyserEventArgs e)
        {
            List<FFTResult> results = new List<FFTResult>();


            for (int i = 0; i < e.Result.FFTResults.Length; i++)
            {
                if (e.Result.FFTResults[i].Frequency <= MAX_FREQUENCY)
                {
                    results.Add(e.Result.FFTResults[i]);
                }
            }


            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i].Color = Color.White;
                Lines[i].Text = null;
            }

            double maxDb = -90;
            int maxI = 0;

            int x = 0;
            for (int i = 0; i < results.Count; i++)
            {
                int relativeDb = (int)(100 + results[i].Intensity * 5000); // change me this

                Lines[i].Start = new Vector2(Position.X + x, Position.Y + HEIGHT);
                Lines[i].End = new Vector2(Position.X + x, Position.Y + HEIGHT - relativeDb);
                x += WIDTH / results.Count;

                if (results[i].IsSignificant())
                {
                    Lines[i].Text = results[i].Frequency.ToString();
                }

                if (results[i].DB > maxDb)
                {
                    maxDb = results[i].DB;
                    maxI = i;
                }

            }
         
            Console.WriteLine(e.Result);
            //  var test = results.OrderByDescending(w => w.DB);

            //  Console.WriteLine(string.Join(",", test.Take(5).ToArray().Select(a=>a.Frequency)));
            // Lines[maxI].Color = Color.Red;
            // Lines[maxI].Text = "Max Frequency :" + results[maxI].Frequency;
            //  if (test.FirstOrDefault().Frequency != 0)
            //  Thread.Sleep(500);
            var note = NotesManager.GetNoteByFrequency(results[maxI].Frequency);

            if (note != null)
            {
                //     Lines[maxI].Text = "Note: " + note.Symbol;
            }
            //  Console.WriteLine(results[maxI].Frequency);
        }

        public void Start()
        {
            Analyser.Play();
        }
        public void Draw()
        {
            Debug.DrawRectangle(Rectangle, Color.White, 1);

            foreach (var line in Lines)
            {
                line.Draw();
            }

            Debug.DrawText("FFT Length : " + Analyser.FFTLength, new Vector2(), Color.White);
            Debug.DrawText("Sample Rate : " + Analyser.SampleRate, new Vector2(0, 20), Color.White);
            Debug.DrawText("Max Frequency:" + MAX_FREQUENCY, new Vector2(0, 40), Color.White);

        }
    }
}
