using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DeLauncherForm
{
    [XmlInclude(typeof(BPatch))]
    [XmlInclude(typeof(HPatch))]
    [XmlInclude(typeof(Vanilla))]
    [XmlInclude(typeof(None))]
    public abstract class Patch
    {
        [XmlElement("PatchTag")]

        public abstract string Name { get; }
        public abstract string[] PatchTags { get;}
        public abstract int PatchVersion { get; set; }
        public abstract string Repository { get; }
        public abstract string[] ExceptionFiles { get; }
    }
}
