using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeLauncherForm.Windows
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public event Action<FormConfiguration, LaunchOptions> ApplyOptions;
        public event Action CloseWindow;
        private FormConfiguration configuration;
        private LaunchOptions options;

        public Options(FormConfiguration cfg, LaunchOptions opt)
        {
            InitializeComponent();
            configuration = cfg;
            options = opt;            

            if (configuration.Lang == DeLauncherForm.Language.Rus)
                SetRus();

            SetOptions();
            SetButtonsBindings();
        }

        public void Applying(object sender, EventArgs e)
        {
            ApplyOptions(configuration, options);
            this.Close();
        }

        public void Close(object sender, EventArgs e)
        {
            CloseWindow();
        }

        private void SetRus()
        {
            exeInfo.Text = "Опции Exe файла:";
            modded.Content = "Использовать modded.exe";
            original.Content = "Использовать generals.exe";

            fixFileInfo.Text = "Файл-фикс:";
            fixFile.Content = "Использовать d3d8to9.dll";

            gentoolInfo.Text = "Опции Gentools:";
            AutoUpdate.Content = "Автообновление до последней версии";
            CurrentVersion.Content = "Использовать текущую версию";
            Remove.Content = "Отключить Gentools при запуске";

            dbgInfo.Text = "Фикс AMD краша:";
            dbgHelpRemove.Content = "Удалить dbghelp.dll";

            Apply.Content = "Применить";
        }

        private void SetOptions()
        {
            if (options.ModdedExe)
                modded.IsChecked = true;
            else
                original.IsChecked = true;

            if (options.FixFile)
                fixFile.IsChecked = true;

            if (options.Gentool == GentoolsMode.AutoUpdate)
                AutoUpdate.IsChecked = true;
            if (options.Gentool == GentoolsMode.Current)
                CurrentVersion.IsChecked = true;
            if (options.Gentool == GentoolsMode.Disable)
                Remove.IsChecked = true;

            if (options.DebugFile == true)
                dbgHelpRemove.IsChecked = true;
        }
        private void SetButtonsBindings()
        {
            dbgHelpRemove.Visibility = Visibility.Collapsed;
            dbgInfo.Visibility = Visibility.Collapsed;
            dbg.Visibility = Visibility.Collapsed;

            modded.Click += ModdedSet;
            original.Click += OriginalSet;
            fixFile.Click += FixFileSet;
            AutoUpdate.Click += AutoUpdateSet;
            CurrentVersion.Click += CurrentVersionSet;
            Remove.Click += RemoveSet;
            dbgHelpRemove.Click += dbgHelpSet;
            Apply.Click += Applying;
            this.Closing += Close;
        }

        private void ModdedSet(object sender, EventArgs e)
        {
            options.ModdedExe = true;
        }

        private void OriginalSet(object sender, EventArgs e)
        {
            options.ModdedExe = false;
        }

        private void FixFileSet(object sender, EventArgs e)
        {
            options.FixFile = !options.FixFile;
        }

        private void AutoUpdateSet(object sender, EventArgs e)
        {
            options.Gentool = GentoolsMode.AutoUpdate;
        }

        private void CurrentVersionSet(object sender, EventArgs e)
        {
            options.Gentool = GentoolsMode.Current;
        }

        private void RemoveSet(object sender, EventArgs e)
        {
            options.Gentool = GentoolsMode.Disable;
        }

        private void dbgHelpSet(object sender, EventArgs e)
        {
            options.DebugFile = !options.DebugFile;
        }

    }
}
