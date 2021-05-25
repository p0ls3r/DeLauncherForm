using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeLauncherForm
{
    public static class SoundsExtractor
    {
        public static void ExtractSounds()
        {
            if (!File.Exists(EntryPoint.LauncherFolder + "press2.wav"))
            {
                System.Windows.Resources.StreamResourceInfo res = Application.GetResourceStream(new Uri("resources/press2.wav", UriKind.Relative));
                res.Stream.CopyTo(new System.IO.FileStream(EntryPoint.LauncherFolder + "press2.wav", System.IO.FileMode.OpenOrCreate));
            }

            if (!File.Exists(EntryPoint.LauncherFolder + "press3.wav"))
            {
                System.Windows.Resources.StreamResourceInfo res = Application.GetResourceStream(new Uri("resources/press3.wav", UriKind.Relative));
                res.Stream.CopyTo(new System.IO.FileStream(EntryPoint.LauncherFolder + "press3.wav", System.IO.FileMode.OpenOrCreate));
            }

            if (!File.Exists(EntryPoint.LauncherFolder + "press1_new.wav"))
            {
                System.Windows.Resources.StreamResourceInfo res = Application.GetResourceStream(new Uri("resources/press1_new.wav", UriKind.Relative));
                res.Stream.CopyTo(new System.IO.FileStream(EntryPoint.LauncherFolder + "press1_new.wav", System.IO.FileMode.OpenOrCreate));
            }

            if (!File.Exists(EntryPoint.LauncherFolder + "press4.wav"))
            {
                System.Windows.Resources.StreamResourceInfo res = Application.GetResourceStream(new Uri("resources/press4.wav", UriKind.Relative));
                res.Stream.CopyTo(new System.IO.FileStream(EntryPoint.LauncherFolder + "press4.wav", System.IO.FileMode.OpenOrCreate));
            }
        }
    }
}
