using System;
using System.Collections.Generic;

namespace DeLauncherForm
{
    [Serializable]
    public class FormConfiguration
    {
        public bool Windowed { get; set; }
        public bool QuickStart { get; set; }
        public Language Lang { get; set; }
        public Patch Patch { get; set; }
        public bool ManualFile { get; set; } = false;

        public List<string> PreviousActivatedFiles = new List<string>();
        public bool scriptDebug { get; set; }

        public bool particleEdit { get; set; }
    }

    public enum Language : byte
    {
        Rus,
        Eng,
    }
}
