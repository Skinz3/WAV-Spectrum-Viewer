using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocalyz.Music
{
    public class NotesCorrelationManager
    {
        public static List<NotePattern> Patterns = new List<NotePattern>();

        public static void Initialize()
        {
            Patterns.Add(new NotePattern("F#5", 363, 375, 386));
            Patterns.Add(new NotePattern("D#5", 609, 621, 632));
            Patterns.Add(new NotePattern("D#3", 152, 164));
        }
    }
    public class NotePattern
    {
        public NotePattern(string noteString, params int[] frequencies)
        {
            Note = NotesManager.GetNoteByString(noteString);

            if (Note == null)
            {
                throw new Exception("Note is not defined : " + noteString);
            }

            Frequencies = frequencies;
        }
        public Note Note
        {
            get;
            set;
        }
        public int[] Frequencies
        {
            get;
            set;
        }

    }
}
