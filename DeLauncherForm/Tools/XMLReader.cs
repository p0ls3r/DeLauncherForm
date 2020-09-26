using System.IO;
using System.Xml.Serialization;


namespace DeLauncherForm
{
    static class XMLReader
    {
        static public LaunchOptions ReadOptions()
        {
            var path = EntryPoint.LauncherFolder;
            if (!Directory.Exists(path))
                CreateLauncherFolder();

            if (!File.Exists(EntryPoint.optionsName))
                CreateNewOptionsFile();

            var formatter = new XmlSerializer(typeof(LaunchOptions));

            LaunchOptions options;

            using (FileStream fs = new FileStream(EntryPoint.optionsName, FileMode.Open))
            {
                options = (LaunchOptions)formatter.Deserialize(fs);
            }

            return options;
        }

        static private void CreateNewOptionsFile()
        {
            var opt = new LaunchOptions
            {
                ModdedExe = true,
                FixFile = false,
                Gentool = GentoolsMode.Current,
                DebugFile = true                
            };

            var formatter = new XmlSerializer(typeof(LaunchOptions));

            using (var fs = new FileStream(EntryPoint.optionsName, FileMode.Create))
            {
                formatter.Serialize(fs, opt);
            }
        }

        static public void SaveOptions(LaunchOptions opt)
        {
            File.Delete(EntryPoint.optionsName);

            var formatter = new XmlSerializer(typeof(LaunchOptions));

            using (var fs = new FileStream(EntryPoint.optionsName, FileMode.Create))
            {
                formatter.Serialize(fs, opt);
            }
        }

        static public FormConfiguration ReadConfiguration()
        {
            var path = EntryPoint.LauncherFolder;
            if (!Directory.Exists(path))
                CreateLauncherFolder();

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
        private static void CreateLauncherFolder()
        {
            var path = EntryPoint.LauncherFolder;

            var folder = Directory.CreateDirectory(path);
            folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }
    }
}
