using System.Collections.Generic;
using System;

namespace DeLauncherForm
{
    [Serializable]
    public class HPatch : Patch
    {
        public override string Name => "HP";
        public override string[] PatchTags { get; } = new string[] { "HP" };

        public override string[] ExceptionFiles { get; } = new string[] { };
        public override int PatchVersion { get; set; }
        public override string Repository { get; } = EntryPoint.HPLink;

        public HPatch(int version)
        {
            PatchVersion = version;
        }
        public HPatch()
        {
        }
    }
}
