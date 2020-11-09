using System.Collections.Generic;
using System;

namespace DeLauncherForm
{
    [Serializable]
    public class None : Patch
    {
        public override string Name => "None";
        public override string[] PatchTags { get; } = new string[] { "" };

        public override string[] ExceptionFiles { get; } = new string[] { };
        public override string Repository { get; } = "";
        public override int PatchVersion { get; set; } = -1;
    }
}
