using System;
using System.Threading.Tasks;
using System.Windows;


namespace DeLauncherForm
{
    public partial class MainWindow : Window
    {
        private FormConfiguration conf;
        public MainWindow(FormConfiguration cfg)
        {
            conf = cfg;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            LocalFilesWorker.ClearTempFiles();

            InitializeComponent();
            ApplyConfig();

            if (ConnectionChecker.CheckInternet() == ConnectionChecker.ConnectionStatus.LimitedAccess || ConnectionChecker.CheckInternet() == ConnectionChecker.ConnectionStatus.NotConnected)
                NoInternetSettings();
            else
                CheckAndUpdate();

            SetButtonsBindings();
        }

        private async void CheckAndUpdate()
        {
            var latestVersion = Updater.GetLatestVersionNumber();
            var currentVersion = VersionChecker.GetVersionNumber();

            if (currentVersion < latestVersion)
                await Task.Run(() => Updater.DownloadUpdate());
        }

        private void NoInternetSettings()
        {
            NoUpd.IsChecked = true;
            conf.Patch = new None();
            HP.Visibility = Visibility.Collapsed;
            BP.Visibility = Visibility.Collapsed;
            Vanilla.Visibility = Visibility.Collapsed;
            Info.Visibility = Visibility.Collapsed;

            NoInternet.Visibility = Visibility.Visible;
            NoInternet2.Visibility = Visibility.Visible;            
        }

        private void SetButtonsBindings()
        {
            launch.Click += Launch;
            Windowed.Click += WindowSet;
            QuickStart.Click += QuickStartSet;
            BP.Click += BPSet;
            HP.Click += HPSet;
            Vanilla.Click += VanillaSet;
            Rus.Click += RusSet;
            Eng.Click += EngSet;
            NoUpd.Click += NonUpdateSet;
            WorldBuilder.Click += LaunchWorldBuilder;

            this.Closing += SaveConfig;
        }
        private void ApplyConfig()
        {
            if (conf.QuickStart)
                QuickStart.IsChecked = true;
            if (conf.Windowed)
                Windowed.IsChecked = true;

            if (conf.Patch is HPatch)
                HP.IsChecked = true;
            if (conf.Patch is BPatch)
                BP.IsChecked = true;
            if (conf.Patch is Vanilla)
                Vanilla.IsChecked = true;
            if (conf.Patch is None)
                NoUpd.IsChecked = true;

            if(conf.Lang == DeLauncherForm.Language.Eng)
            {
                Eng.IsChecked = true;
                SetEngLang();
            }

            if (conf.Lang == DeLauncherForm.Language.Rus)
            {
                Rus.IsChecked = true;
                SetRusLang();
            }

            VersionInfo.Text = "Ver: "+typeof(EntryPoint).Assembly.GetName().Version.ToString();
        }

        private void SetRusLang()
        {
            launch.Content = "Запуск";            
            QuickStart.Content = "Быстрый старт";
            Windowed.Content = "Запуск в окне";

            Vanilla.Content = "Оригинальный ROTR";
            BP.Content = "БалансПатч (ИИ не работает)";
            HP.Content = "ХанПатч";
            NoUpd.Content = "Текущий патч без загрузки";
            Info.Text = "Выбор патча для автообновления:";

            PatchInfo.Text = "Текущий Файл Патча: " + LocalFilesWorker.GetCurrentPatchName();

            NoInternet.Text = "Нет доступа к репозиторию";
            NoInternet2.Text = "Автообновления недоступны!";
        }

        private void SetEngLang()
        {
            QuickStart.Content = "Quick start";
            launch.Content = "Launch";
            Windowed.Content = "Windowed";

            Vanilla.Content = "Vanilla ROTR";
            BP.Content = "BalancePatch (AI Incompatible)";
            HP.Content = "HanPatch";
            NoUpd.Content = "Current patch without load";
            Info.Text = "Choose patch for auto update:";

            PatchInfo.Text = "Current patchFile: " + LocalFilesWorker.GetCurrentPatchName();

            NoInternet.Text = "No connection to repository";
            NoInternet2.Text = "Updates are not available!";
        }
        private void SaveConfig(object sender, EventArgs e)
        {
            ConfigurationReader.SaveConfiguration(conf);
        }

        private async void LaunchWorldBuilder(object sender, RoutedEventArgs e)
        {
            this.Hide();
            await Task.Run(() => WorldBuilderLauncher.LaunchWorldBuilder());
            SaveConfig(this, null);
            this.Close();
        }

        private void Launch(object sender, RoutedEventArgs e)
        {
            var curVersion = LocalFilesWorker.GetCurrentPatchInfo();

            if (conf.Patch is None)
            {
                LaunchWithoutUpdate(conf, curVersion);
                return;
            }
            
            var reposVersion = ReposWorker.GetLatestPatchInfo(conf);            

            if (curVersion.Patch.Name == conf.Patch.Name && curVersion.Patch.PatchVersion == reposVersion.Patch.PatchVersion)
            {
                LaunchWithoutUpdate(conf, curVersion);
                return;
            }

            if (LocalFilesWorker.CheckGibForActualVersion(reposVersion))
            {
                LaunchWithoutUpdate(conf, reposVersion);
                return;
            }

            LaunchWithUpdate(conf);           
        }
        private async void LaunchWithoutUpdate(FormConfiguration conf, PatchInfo info)
        {
            this.Hide();
            await Task.Run(() => GameLauncher.PrepareWithoutUpdate(conf, info));
            await Task.Run(() => GameLauncher.Launch(conf));
            SaveConfig(this, null);
            this.Close();
        }

        private async void LaunchWithUpdate(FormConfiguration conf)
        {
            DownloadWindow downloadWindow = new DownloadWindow(conf);
            downloadWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            ReposWorker.DownloadStatusChanged += downloadWindow.UpdateInformation;
            downloadWindow.Show();

            this.Hide();

            await GameLauncher.PrepareWithUpdate(conf);
            downloadWindow.Hide();
            await Task.Run(() => GameLauncher.Launch(conf));

            SaveConfig(this, null);
            downloadWindow.Close();
            this.Close();
        }


        private void WindowSet(object sender, RoutedEventArgs e)
        {
            conf.Windowed = !conf.Windowed;
        }

        private void QuickStartSet(object sender, RoutedEventArgs e)
        {
            conf.QuickStart = !conf.QuickStart;
        }

        private void BPSet(object sender, RoutedEventArgs e)
        {
            conf.Patch = new BPatch();
        }

        private void HPSet(object sender, RoutedEventArgs e)
        {
            conf.Patch = new HPatch();
        }

        private void VanillaSet(object sender, RoutedEventArgs e)
        {
            conf.Patch = new Vanilla();
        }

        private void NonUpdateSet(object sender, RoutedEventArgs e)
        {
            conf.Patch = new None();
        }

        private void RusSet(object sender, RoutedEventArgs e)
        {
            conf.Lang = DeLauncherForm.Language.Rus;
            SetRusLang();
        }

        private void EngSet(object sender, RoutedEventArgs e)
        {
            conf.Lang = DeLauncherForm.Language.Eng;
            SetEngLang();
        }
    }
}
