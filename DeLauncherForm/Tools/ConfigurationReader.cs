using System.IO;
using System.Xml.Serialization;


namespace DeLauncherForm
{
    static class ConfigurationReader
    {
        static public FormConfiguration ReadConfiguration()
        {
            if (!File.Exists(EntryPoint.configName))
                CreateNewConfiguration();

            var formatter = new XmlSerializer(typeof(FormConfiguration));

            FormConfiguration config;

            using (FileStream fs = new FileStream(EntryPoint.configName, FileMode.Open))
            {
                config = (FormConfiguration)formatter.Deserialize(fs);
            }

            return config;
        }

        static public void SaveConfiguration(FormConfiguration conf)
        {
            File.Delete(EntryPoint.configName);

            var formatter = new XmlSerializer(typeof(FormConfiguration));

            using (var fs = new FileStream(EntryPoint.configName, FileMode.Create))
            {
                formatter.Serialize(fs, conf);
            }
        }

        static private void CreateNewConfiguration()
        {
            var conf = new FormConfiguration
            {
                Patch = new None(),
                Lang = Language.Eng
            };

            var formatter = new XmlSerializer(typeof(FormConfiguration));

            using (var fs = new FileStream(EntryPoint.configName, FileMode.Create))
            {
                formatter.Serialize(fs, conf);
            }
        }
    }
}
