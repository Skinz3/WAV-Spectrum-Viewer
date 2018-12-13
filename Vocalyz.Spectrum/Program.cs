using NAudio.Midi;
using System;
using System.Collections.Generic;
using Vocalyz.Midi;

namespace Vocalyz.Spectrum
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            using (var game = new SpectrumView())
                game.Run();
        }
    }
#endif
}
