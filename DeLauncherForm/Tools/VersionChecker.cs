using System.IO;
using System.Xml;

namespace DeLauncherForm
{
    class VersionChecker
    {
        public static int GetVersionNumber()
        {
            var path = EntryPoint.LauncherFolder;
            if (!Directory.Exists(path))
                CreateLauncherFolder();

            if (!File.Exists(path+EntryPoint.VersionFileName))
                CreateVersionFile();

            return XmlVersionReader.GetVersionFromXml(path + EntryPoint.VersionFileName);
        }

        private static void CreateVersionFile()
        {
            var path = EntryPoint.LauncherFolder;
            var defaultNumber = EntryPoint.DefaultVersionNumber.ToString();

            var xDoc = new XmlDocument();
            var verElem = xDoc.CreateElement("version");
            var numberElem = xDoc.CreateElement("number");

            XmlText ageText = xDoc.CreateTextNode(defaultNumber);

            numberElem.AppendChild(ageText);
            verElem.AppendChild(numberElem);
            xDoc.AppendChild(verElem);

            xDoc.Save(path + EntryPoint.VersionFileName);
        }
        private static void CreateLauncherFolder()
        {
            var path = EntryPoint.LauncherFolder;

            var folder = Directory.CreateDirectory(path);
            folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }
    }
}
