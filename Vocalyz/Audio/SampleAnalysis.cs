using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocalyz.Music;

namespace Vocalyz.Audio
{
    public class SampleAnalysis
    {
        public FFTResult[] FFTResults
        {
            get;
            private set;
        }

        public SampleAnalysis(FFTResult[] fftResults)
        {
            this.FFTResults = fftResults;
        }
        public FFTResult GetFrequencyPeak()
        {
            return FFTResults.OrderByDescending(x => x.Intensity).FirstOrDefault();
        }
        public Note GetNoteByFrequencyPeak()
        {
            return NotesManager.GetNoteByFrequency(GetFrequencyPeak().Frequency);
        }
        public bool IsSignificant(int frequency)
        {
            return FFTResults.FirstOrDefault(x => x.Frequency == frequency).IsSignificant();
        }
        public Note[] GetNotes()
        {
            List<Note> notes = new List<Note>();

            foreach (var pattern in NotesCorrelationManager.Patterns)
            {
                if (pattern.Frequencies.All(x => IsSignificant(x)))
                {
                    notes.Add(pattern.Note);
                }
               
            }
            return notes.ToArray();
        }
        public override string ToString()
        {
            return string.Join(",", GetNotes().AsEnumerable());
        }
    }

}
