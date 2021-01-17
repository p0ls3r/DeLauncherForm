using System.Collections.Generic;
using System;

namespace DeLauncherForm
{
    [Serializable]
    public class Vanilla : Patch
    {
        public override string Name => "Original ROTR";
        public override string[] PatchTags { get; } = new string[] { "!!Rotr_Intrnl_INI", "!!Rotr_Intrnl_Eng" };

        public override string[] ExceptionFiles { get; } = new string[] { };
        public override string Repository { get; } = EntryPoint.VanillaLink;
        public override int PatchVersion { get; set; } = 18720;

        public Vanilla()
        {
        }
    }
}
