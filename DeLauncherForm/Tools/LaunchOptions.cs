using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    [Serializable]
    public class LaunchOptions
    {
        public bool ModdedExe { get; set; }
        public bool FixFile { get; set; }
        public GentoolsMode Gentool { get; set; }
        public bool DebugFile { get; set; }
        public bool DeleteOldVersions { get; set; } = true;
        public bool Sounds { get; set; } = true;        
    }

    public enum GentoolsMode : byte
    {
        AutoUpdate,
        Current,
        Disable
    }
}
