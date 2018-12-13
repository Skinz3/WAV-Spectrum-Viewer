using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vocalyz.Midi
{
    public class Song
    {
        Dictionary<int, int[]> notes = new Dictionary<int, int[]>();

        public Song(Dictionary<int, int[]> notes)
        {
            this.notes = notes;
        }

        public void Play()
        {
            var midiOut = new MidiOut(0);
            int channel = 1;



            foreach (var note in notes)
            {
                foreach (var n in note.Value)
                {
                    var noteOnEvent = new NoteOnEvent(0, channel, n, 127, 50);
                    midiOut.Send(noteOnEvent.GetAsShortMessage());
                }
                Thread.Sleep(500);

            }


        }

    }
}
