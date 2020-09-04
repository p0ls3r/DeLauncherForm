using System;

namespace DeLauncherForm
{
    [Serializable]
    public class FormConfiguration
    {
        public bool Windowed { get; set; }
        public bool QuickStart { get; set; }
        public Language Lang { get; set; }
        public Patch Patch { get; set; }
    }

    public enum Language : byte
    {
        Rus,
        Eng,
    }
}
