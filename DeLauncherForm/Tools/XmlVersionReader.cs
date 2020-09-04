using System.Xml;

namespace DeLauncherForm
{
    static class XmlVersionReader
    {
        public static int GetVersionFromXml(string path)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(path);

            var textNumber = xDoc.FirstChild.InnerText;
            var numbers = textNumber.Split('.');
            var result = "";

            foreach (var number in numbers)
            {
                result += number;
            }

            return int.Parse(result);
        }
    }
}
