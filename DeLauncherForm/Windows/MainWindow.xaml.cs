using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace DeLauncherForm
{
    public partial class MainWindow : Window
    {
        private FormConfiguration configuration;
        private LaunchOptions options;

        private bool noInternet = false;        

        public MainWindow(FormConfiguration cfg, LaunchOptions opt)
        {
            configuration = cfg;
            options = opt;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            LocalFilesWorker.ClearTempFiles();

            InitializeComponent();
            ApplyConfig();

            var connection = ConnectionChecker.CheckConnection("https://github.com/").GetAwaiter().GetResult();

            if (connection == ConnectionChecker.ConnectionStatus.NotConnected)
                NoInternetSettings();
            else
                CheckAndUpdate();
            
            SetButtonsBindings();            
        }

        public void ShowWindow(FormConfiguration cfg, LaunchOptions opt)
        {
            this.Show();            
            configuration = cfg;
            options = opt;
        }

        public void ShowWindow()
        {
            this.Show();
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
            noInternet = true;
            configuration.Patch = new None();
            HP.Visibility = Visibility.Collapsed;
            BP.Visibility = Visibility.Collapsed;
            Vanilla.Visibility = Visibility.Collapsed;
            Info.Visibility = Visibility.Collapsed;

            NoInternet.Visibility = Visibility.Visible;
            NoInternet2.Visibility = Visibility.Visible;

            AdvancedOptions.Visibility = Visibility.Collapsed;

            HPchangeLog.Visibility = Visibility.Collapsed;
            BPchangeLog.Visibility = Visibility.Collapsed;
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
            AdvancedOptions.Click += AdvancedOptionsWindow;
            HPchangeLog.Click += OpenHPChangeLog;
            BPchangeLog.Click += OpenBPChangeLog;

            this.Closing += SaveConfigAndOptions;
        }

        private void AdvancedOptionsWindow(object sender, EventArgs e)
        {
            this.Hide();
            Windows.Options optionsWindow = new Windows.Options(configuration, options)
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,                
            };

            optionsWindow.ApplyOptions += ShowWindow;
            optionsWindow.CloseWindow += ShowWindow;
            optionsWindow.Show();
        }

        private void ApplyConfig()
        {
            if (configuration.QuickStart)
                QuickStart.IsChecked = true;
            if (configuration.Windowed)
                Windowed.IsChecked = true;

            if (configuration.Patch is HPatch)
                HP.IsChecked = true;
            if (configuration.Patch is BPatch)
                BP.IsChecked = true;
            if (configuration.Patch is Vanilla)
                Vanilla.IsChecked = true;
            if (configuration.Patch is None)
                NoUpd.IsChecked = true;

            if(configuration.Lang == DeLauncherForm.Language.Eng)
            {
                Eng.IsChecked = true;
                SetEngLang();
            }

            if (configuration.Lang == DeLauncherForm.Language.Rus)
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
            NoUpd.Content = "Текущий патч без обновлений";
            Info.Text = "Выбор патча для автообновления:";

            PatchInfo.Text = "Текущий Файл Патча: " + LocalFilesWorker.GetCurrentPatchName();

            NoInternet.Text = "Нет доступа к репозиторию";
            NoInternet2.Text = "Автообновления недоступны!";

            AdvancedOptions.Content = "Дополнительные опции";

            HPchangeLog.Content = "Изменения";
            BPchangeLog.Content = "Изменения";
        }

        private void SetEngLang()
        {
            QuickStart.Content = "Quick Start";
            launch.Content = "Launch";
            Windowed.Content = "Windowed";

            Vanilla.Content = "Original ROTR";
            BP.Content = "BalancePatch (AI Incompatible)";
            HP.Content = "HanPatch";
            NoUpd.Content = "Current patch without update";
            Info.Text = "Choose patch for auto update:";

            PatchInfo.Text = "Current patchFile: " + LocalFilesWorker.GetCurrentPatchName();

            NoInternet.Text = "No connection to repository";
            NoInternet2.Text = "Updates are not available!";

            AdvancedOptions.Content = "Advanced Options";

            HPchangeLog.Content = "ChangeLog";
            BPchangeLog.Content = "ChangeLog";
        }

        private void SaveConfigAndOptions(object sender, EventArgs e)
        {
            XMLReader.SaveConfiguration(configuration);
            XMLReader.SaveOptions(options);
        }

        private async void LaunchWorldBuilder(object sender, RoutedEventArgs e)
        {
            this.Hide();
            await Task.Run(() => WorldBuilderLauncher.LaunchWorldBuilder(configuration));
            SaveConfigAndOptions(this, null);
            this.Close();
        }

        private async Task CheckAndApplyOptions()
        {
            this.Hide();
            DeLauncherForm.Windows.ApplyingOptions optionsWindow = new DeLauncherForm.Windows.ApplyingOptions(configuration);
            optionsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            optionsWindow.Show();

            var succes = await GentoolsUpdater.CheckAndUpdateGentools(options);

            if (!succes)
            {
                DeLauncherForm.Windows.GentoolUpdateFailed gentoolFailedWindow = new Windows.GentoolUpdateFailed(configuration);
                gentoolFailedWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                gentoolFailedWindow.ShowDialog();            
            }

            await OptionsSetter.CheckAndApplyOptions(options);

            optionsWindow.Close();
        }

        private void Launch(object sender, RoutedEventArgs e)
        {            
            var curVersion = LocalFilesWorker.GetCurrentPatchInfo();

            if (configuration.Patch is None)
            {
                LaunchWithoutUpdate(configuration, curVersion);
                return;
            }
            
            var reposVersion = ReposWorker.GetLatestPatchInfo(configuration);            

            if (curVersion.Patch.Name == configuration.Patch.Name && curVersion.Patch.PatchVersion == reposVersion.Patch.PatchVersion)
            {
                LaunchWithoutUpdate(configuration, curVersion);
                return;
            }

            if (LocalFilesWorker.CheckGibForActualVersion(reposVersion))
            {
                LaunchWithoutUpdate(configuration, reposVersion);
                return;
            }

            LaunchWithUpdate(configuration);           
        }

        private async void LaunchWithoutUpdate(FormConfiguration conf, PatchInfo info)
        {
            if (!noInternet)
                await CheckAndApplyOptions();

            this.Hide();
            await Task.Run(() => GameLauncher.PrepareWithoutUpdate(conf, info));
            await Task.Run(() => GameLauncher.Launch(conf, options));
            SaveConfigAndOptions(this, null);
            this.Close();
        }

        private async void LaunchWithUpdate(FormConfiguration conf)
        {
            if (!noInternet)
                await CheckAndApplyOptions();

            DownloadWindow downloadWindow = new DownloadWindow(conf);
            downloadWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            ReposWorker.DownloadStatusChanged += downloadWindow.UpdateInformation;
            downloadWindow.Show();

            this.Hide();

            await GameLauncher.PrepareWithUpdate(conf);
            downloadWindow.Hide();
            await Task.Run(() => GameLauncher.Launch(conf, options));

            SaveConfigAndOptions(this, null);
            downloadWindow.Close();
            this.Close();
        }


        private void WindowSet(object sender, RoutedEventArgs e)
        {
            configuration.Windowed = !configuration.Windowed;
        }

        private void QuickStartSet(object sender, RoutedEventArgs e)
        {
            configuration.QuickStart = !configuration.QuickStart;
        }

        private void BPSet(object sender, RoutedEventArgs e)
        {
            configuration.Patch = new BPatch();
        }

        private void HPSet(object sender, RoutedEventArgs e)
        {
            configuration.Patch = new HPatch();
        }

        private void VanillaSet(object sender, RoutedEventArgs e)
        {
            configuration.Patch = new Vanilla();
        }

        private void NonUpdateSet(object sender, RoutedEventArgs e)
        {
            configuration.Patch = new None();
        }

        private void OpenHPChangeLog(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(EntryPoint.HPLogURL);
        }

        private void OpenBPChangeLog(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(EntryPoint.BPLogURL);
        }


        private void RusSet(object sender, RoutedEventArgs e)
        {
            configuration.Lang = DeLauncherForm.Language.Rus;
            SetRusLang();
        }

        private void EngSet(object sender, RoutedEventArgs e)
        {
            configuration.Lang = DeLauncherForm.Language.Eng;
            SetEngLang();
        }
    }
}
