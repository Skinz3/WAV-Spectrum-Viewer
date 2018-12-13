using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocalyz.Music;

namespace Vocalyz.Audio
{
    public struct FFTResult
    {
        public int Frequency;

        public double DB;

        public double Intensity;

        public FFTResult(int frequency, double intensity)
        {
            this.Frequency = frequency;
            this.Intensity = intensity;
            this.DB = 10 * Math.Log10(intensity);
        }

        public bool IsSignificant()
        {
            return DB > -25;
        }

        public Note GetNote(int frequencyGap)
        {
            return NotesManager.GetNoteByFrequency(Frequency, frequencyGap);
        }
    }
}
