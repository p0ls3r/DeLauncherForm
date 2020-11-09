using System.Collections.Generic;
using System;

namespace DeLauncherForm
{
    [Serializable]
    public class BPatch: Patch
    {
        public override string Name => "BP";
        public override string[] PatchTags { get; } = new string[] { "BP" };
        public override string[] ExceptionFiles { get; } = new string[] { "!!Rotr_Intrnl_AI" };
        public override int PatchVersion { get; set; }
        public override string Repository { get; } = EntryPoint.BPLink;
        public BPatch(int version)
        {
            PatchVersion = version;
        }
        public BPatch()
        {
        }
    }
}
