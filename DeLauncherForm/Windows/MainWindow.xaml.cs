using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Lng = DeLauncherForm.Language;

namespace DeLauncherForm
{
    public partial class MainWindow : Window
    {
        private FormConfiguration configuration;
        private LaunchOptions options;
        private ImageHandler repos = new ImageHandler();
        public MediaPlayer player1 = new MediaPlayer();
        public MediaPlayer player2 = new MediaPlayer();
        public MediaPlayer player3 = new MediaPlayer();
        public MediaPlayer player4 = new MediaPlayer();

        private bool noInternet = false;
        private int theCode = 0;

        public MainWindow(FormConfiguration cfg, LaunchOptions opt)
        {            
            configuration = cfg;
            options = opt;            
            

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            LocalFilesWorker.ClearTempFiles();
            LocalFilesWorker.ConvertBigsToGibs();

            InitializeComponent();
            ApplyConfig();

            NoInternet.Opacity = 0;

            var connection = ConnectionChecker.CheckConnection("https://github.com/").GetAwaiter().GetResult();
            //var connection = ConnectionChecker.ConnectionStatus.NotConnected;

            if (connection == ConnectionChecker.ConnectionStatus.NotConnected)
                NoInternetMode();
            else
                CheckAndUpdate();

            SetButtonsBindings();           
        }

        #region SoundsHandlers
        private void StopPlayers()
        {
            if (options.Sounds)
            {
                player1.Close();
                player2.Close();
                player3.Close();
                player4.Close();
            }
        }

        private void SetVolume()
        {
            player1.SpeedRatio = 0.85;
            player3.SpeedRatio = 0.9;
            player1.Volume = EntryPoint.Volume1;
            player2.Volume = EntryPoint.Volume2;
            player3.Volume = EntryPoint.Volume1;
        }

        private void GetSound1()
        {
            if (options.Sounds)
            {
                StopPlayers();
                player3.Open(new Uri(EntryPoint.LauncherFolder + "press1_new.wav", UriKind.Relative));
                SetVolume();
                player3.Play();
            }
        }

        private void GetSound2()
        {
            if (options.Sounds)
            {
                StopPlayers();
                player1.Open(new Uri(EntryPoint.LauncherFolder + "press2.wav", UriKind.Relative));
                SetVolume();
                player1.Play();
            }
        }

        private void GetSound3()
        {
            if (options.Sounds)
            {
                StopPlayers();
                player2.Open(new Uri(EntryPoint.LauncherFolder + "press3.wav", UriKind.Relative));
                SetVolume();
                player2.Play();
            }
        }

        private void GetSound4()
        {
            if (options.Sounds)
            {
                StopPlayers();
                player4.Open(new Uri(EntryPoint.LauncherFolder + "press4.wav", UriKind.Relative));
                SetVolume();
                player4.Play();
            }
        }

        private void ClosePlayers(object sender, EventArgs e)
        {
            player1.Volume = 0;
            player2.Volume = 0;
            player3.Volume = 0;
            player4.Volume = 0;
            StopPlayers();
        }

        #endregion

        public void ShowWindow(FormConfiguration cfg, LaunchOptions opt)
        {
            this.Show();
            configuration = cfg;
            options = opt;
            GetSound1();
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

        private void NoInternetMode()
        {
            gifImage.Opacity = 0;
            NoInternet.Opacity = 100;
            SetVanillaShtora();

            AdvancedOptions.Visibility = Visibility.Collapsed;
            ManualFileSelect.Visibility = Visibility.Collapsed;            

            ManualFileMode();
        }

        private void ManualFileMode()
        {
            if (theCode == 3)
            {
                theCode = 4;
            }
            else
                theCode = 0;

            configuration.Patch = new Vanilla();
            configuration.ManualFile = true;
            BackGrondImage2.Visibility = Visibility.Visible;            


            string prevFile = null;
            if (configuration.PreviousActivatedFiles.Count > 0)
               prevFile = configuration.PreviousActivatedFiles[0];

            FilesList.Items.Clear();

            foreach (var file in LocalFilesWorker.GetPatchFileNames())
                FilesList.Items.Add(file);

            var fileFounded = false;

            if (prevFile != null)
                foreach (var file in FilesList.Items)
                {
                    if (file.ToString() == prevFile)
                    {
                        FilesList.SelectedItem = file;
                        configuration.PreviousActivatedFiles.Clear();
                        configuration.PreviousActivatedFiles.Add(file.ToString());
                        fileFounded = true;
                        ClearShtora();
                    }
                }

            if (theCode == 4)
                FilesList.Items.Add("КОД56-24-81АЛЬФА");

            if (!fileFounded)
                SetVanillaShtora();

            BP.Visibility = Visibility.Collapsed;
            HP.Visibility = Visibility.Collapsed;
            FilesList.Visibility = Visibility.Visible;

            ShtoraManual.Visibility = Visibility.Visible;

            HPChanglog.Visibility = Visibility.Collapsed;
            BPChanglog.Visibility = Visibility.Collapsed;
        }

        private void AutoUpdateMode()
        {
            BackGrondImage2.Visibility = Visibility.Hidden;
            BP.Visibility = Visibility.Visible;
            HP.Visibility = Visibility.Visible;

            FilesList.Visibility = Visibility.Collapsed;

            HPChanglog.Visibility = Visibility.Visible;
            BPChanglog.Visibility = Visibility.Visible;            
        }

        #region WindowPreparation
        private void SetButtonsBindings()
        {
            launch.PreviewMouseLeftButtonDown += LaunchStart;
            launch.PreviewMouseLeftButtonUp += LaunchEnd;
            Windowed.PreviewMouseLeftButtonDown += WindowedStart;
            Windowed.PreviewMouseLeftButtonUp += WindowedEnd;
            QuickStart.PreviewMouseLeftButtonDown += QuickStartStart;
            QuickStart.PreviewMouseLeftButtonUp += QuickStartEnd;
            WorldBuilder.PreviewMouseLeftButtonDown += LaunchWorldBuilderStart;
            WorldBuilder.PreviewMouseLeftButtonUp += LaunchWorldBuilderEnd;
            Exit.PreviewMouseLeftButtonDown += GoExitStart;
            Exit.PreviewMouseLeftButtonUp += GoExitEnd;
            BP.PreviewMouseLeftButtonDown += BPSetStart;
            BP.PreviewMouseLeftButtonUp += BPSetEnd;
            HP.PreviewMouseLeftButtonDown += HPSetStart;
            HP.PreviewMouseLeftButtonUp += HPSetEnd;
            Vanilla.PreviewMouseLeftButtonDown += VanillaSetStart;
            Vanilla.PreviewMouseLeftButtonUp += VanillaSetEnd;
            AdvancedOptions.PreviewMouseLeftButtonDown += AdvancedOptionsWindowStart;
            AdvancedOptions.PreviewMouseLeftButtonUp += AdvancedOptionsWindowEnd;
            FilesList.SelectionChanged += FilesListSelectionChanged;

            HPChanglog.PreviewMouseLeftButtonDown += OpenHPChangeLogStart;
            HPChanglog.PreviewMouseLeftButtonUp += OpenHPChangeLogEnd;
            BPChanglog.PreviewMouseLeftButtonDown += OpenBPChangeLogStart;
            BPChanglog.PreviewMouseLeftButtonUp += OpenBPChangeLogEnd;

            ManualFileSelect.PreviewMouseLeftButtonDown += ManualFileSelectStart;
            ManualFileSelect.PreviewMouseLeftButtonUp += ManualFileSelectEnd;


            this.MouseDown += Window_MouseDown;
            this.Closing += SaveConfigAndOptions;
            this.Unloaded += ClosePlayers;

            Rus.Click += RusSet;
            Eng.Click += EngSet;
        }        

        private void ApplyConfig()
        {
            QuickStartIndicatorStatusChange(configuration.QuickStart);
            WindowedIndicatorStatusChange(configuration.Windowed);

            gifImage.GifSource = "/monitor.gif";
            gifImage.AutoStart = true;

            if (configuration.Patch is HPatch)
                SetHPShtora();
            if (configuration.Patch is BPatch)
                SetBPShtora();
            if (configuration.Patch is Vanilla)
                SetVanillaShtora();
            if (configuration.Patch is None)
            {
                SetVanillaShtora();
                configuration.Patch = new None();
            }

            if (configuration.Lang == DeLauncherForm.Language.Eng)
                SetEngLang();


            if (configuration.Lang == DeLauncherForm.Language.Rus)
                SetRusLang();            


            if (!configuration.ManualFile)
                FilesList.Visibility = Visibility.Hidden;
            else
                ManualFileMode();


            VersionInfo.Content = typeof(EntryPoint).Assembly.GetName().Version.ToString();
        }

        private void SetRusLang()
        {
            var r = Lng.Rus;

            launchSource.Source = repos.GetImage(false, r, "launch");
            QuickStartSource.Source = repos.GetImage(false, r, "quickstart");
            WindowedSource.Source = repos.GetImage(false, r, "windowed");
            ExitSource.Source = repos.GetImage(false, r, "exit");
            ManualFileSelectSource.Source = repos.GetImage(false, r, "manual");

            BPSource.Source = repos.GetImage(false, r, "BP");
            HPSource.Source = repos.GetImage(false, r, "HP");
            VanillaSource.Source = repos.GetImage(false, r, "vanilla");

            HPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            BPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            WorldbuilderSource.Source = repos.GetImage(false, Lng.Eng, "worldbuilder");

            OptionsSource.Source = repos.GetImage(false, r, "options");

            InfoAll.Source = new BitmapImage(new Uri("/Windows/Resources/Main/info_r.png", UriKind.Relative));

            NoInternet.Source = new BitmapImage(new Uri("/Windows/Resources/Main/nointernet_r.png", UriKind.Relative));

            RusImage.Visibility = Visibility.Visible;
            EngImage.Visibility = Visibility.Hidden;
        }

        private void SetEngLang()
        {
            var e = Lng.Eng;

            launchSource.Source = repos.GetImage(false, e, "launch");
            QuickStartSource.Source = repos.GetImage(false, e, "quickstart");
            WindowedSource.Source = repos.GetImage(false, e, "windowed");
            ExitSource.Source = repos.GetImage(false, e, "exit");

            ManualFileSelectSource.Source = repos.GetImage(false, e, "manual");
            HPChangelogSource.Source = repos.GetImage(false, e, "changelog");
            BPChangelogSource.Source = repos.GetImage(false, e, "changelog");
            WorldbuilderSource.Source = repos.GetImage(false, e, "worldbuilder");

            BPSource.Source = repos.GetImage(false, e, "BP");
            HPSource.Source = repos.GetImage(false, e, "HP");
            VanillaSource.Source = repos.GetImage(false, e, "vanilla");

            OptionsSource.Source = repos.GetImage(false, e, "options");

            RusImage.Visibility = Visibility.Hidden;
            EngImage.Visibility = Visibility.Visible;

            InfoAll.Source = new BitmapImage(new Uri("/Windows/Resources/Main/info_e.png", UriKind.Relative));

            NoInternet.Source = new BitmapImage(new Uri("/Windows/Resources/Main/nointernet_e.png", UriKind.Relative));
        }

        #endregion

        private void AdvancedOptionsWindowStart(object sender, EventArgs e)
        {
            OptionsSource.Source = repos.GetImage(true, configuration.Lang, "options");
        }

        private void AdvancedOptionsWindowEnd(object sender, EventArgs e)
        {
            theCode = 0;
            OptionsSource.Source = repos.GetImage(false, configuration.Lang, "options");
            this.Hide();
            Windows.Options optionsWindow = new Windows.Options(configuration, options, repos)
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
            };

            optionsWindow.ApplyOptions += ShowWindow;
            optionsWindow.CloseWindow += ShowWindow;
            optionsWindow.Show();
        }        

        private void SaveConfigAndOptions(object sender, EventArgs e)
        {
            configuration.PreviousActivatedFiles.Clear();
            if (FilesList.SelectedItem != null)
              configuration.PreviousActivatedFiles.Add(FilesList.SelectedItem.ToString());

            XmlData.SaveConfiguration(configuration);
            XmlData.SaveOptions(options);
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

        #region LaunchLogic
        private void Launch(bool worldBuilderLaunch)
        {
            if (configuration.ManualFile && FilesList.SelectedItem != null && FilesList.SelectedItem.ToString() == "КОД56-24-81АЛЬФА")
            {
                GetSound4();
                System.Diagnostics.Process.Start(EntryPoint.BPLogUL);
                return;
            }
            theCode = 0;

            //кейс нет интернета и/или мануал мод
            if (configuration.ManualFile && FilesList.SelectedItem != null)
            {
                var fileName = FilesList.SelectedItem.ToString();

                LaunchManualSelectedFile(fileName, worldBuilderLaunch);

                return;
            }

            if (configuration.ManualFile && FilesList.SelectedItem == null)
            {
                LaunchWithoutUpdate(worldBuilderLaunch);
                return;
            }

            if (LocalFilesWorker.CheckPatchFileExist(configuration.Patch) && (LocalFilesWorker.GetCurrentVersionNumber(configuration.Patch) < ReposWorker.GetLatestPatchNumber(configuration.Patch)))
            {
                LaunchWithUpdate(worldBuilderLaunch);
                return;
            }

            if (!LocalFilesWorker.CheckPatchFileExist(configuration.Patch))
            {
                LaunchWithUpdate(worldBuilderLaunch);
                return;
            }

            LaunchWithoutUpdate(worldBuilderLaunch);
        }


        private async void LaunchManualSelectedFile(string fileName,bool worldBuilderLaunch)
        {
            if (!noInternet && !worldBuilderLaunch)
                await CheckAndApplyOptions();

            if (fileName.Contains("HP"))
                configuration.Patch = new HPatch();

            if (fileName.Contains("BP"))
                configuration.Patch = new BPatch();

            LocalFilesWorker.ActivateFileByName(fileName);

            this.Hide();

            SaveConfigAndOptions(this, null);
            await Task.Run(() => GameLauncher.Launch(configuration, options, worldBuilderLaunch));            
            this.Close();
        }

        private async void LaunchWithoutUpdate(bool worldBuilderLaunch)
        {
            if (!noInternet && !worldBuilderLaunch)
                await CheckAndApplyOptions();

            this.Hide();

            await Task.Run(() =>
            {
                foreach (var file in LocalFilesWorker.GetLatestPatchFileNames(configuration.Patch))
                    LocalFilesWorker.ActivateFileByName(file);
            });

            SaveConfigAndOptions(this, null);
            await Task.Run(() => GameLauncher.Launch(configuration, options, worldBuilderLaunch));            
            this.Close();
        }

        private async void LaunchWithUpdate(bool worldBuilderLaunch)
        {            
            if (!noInternet && !worldBuilderLaunch)
                await CheckAndApplyOptions();

            DownloadWindow downloadWindow = new DownloadWindow(configuration);
            downloadWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            ReposWorker.DownloadStatusChanged += downloadWindow.UpdateInformation;
            downloadWindow.Show();

            this.Hide();

            await GameLauncher.PrepareWithUpdate(configuration);
            downloadWindow.Hide();

            LocalFilesWorker.ConvertBigsToGibs();
            foreach (var file in LocalFilesWorker.GetLatestPatchFileNames(configuration.Patch))
            {
                if (!String.IsNullOrEmpty(file))
                  LocalFilesWorker.ActivateFileByName(file);
            }

            SaveConfigAndOptions(this, null);
            await Task.Run(() => GameLauncher.Launch(configuration, options, worldBuilderLaunch));
            
            downloadWindow.Close();
            this.Close();
        }

        #endregion

        #region WindowHandlers
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void LaunchWorldBuilderStart(object sender, RoutedEventArgs e)
        {
            WorldbuilderSource.Source = repos.GetImage(true, Lng.Eng, "worldbuilder");
        }
        private void LaunchWorldBuilderEnd(object sender, RoutedEventArgs e)
        {
            WorldbuilderSource.Source = repos.GetImage(false, Lng.Eng, "worldbuilder");
            //запускаем ворлд билдер
            GetSound1();
            Launch(true);
        }
        private void LaunchStart(object sender, RoutedEventArgs e)
        {
            launchSource.Source = repos.GetImage(true, configuration.Lang, "launch");
        }
        private void LaunchEnd(object sender, RoutedEventArgs e)
        {
            launchSource.Source = repos.GetImage(false, configuration.Lang, "launch");
            GetSound1();
            //запускаем игру
            Launch(false);
        }
        private void SetHPShtora()
        {
            ClearShtora();
            ShtoraHP.Visibility = Visibility.Visible;
        }

        private void SetBPShtora()
        {
            ClearShtora();
            ShtoraBP.Visibility = Visibility.Visible;
        }

        private void SetVanillaShtora()
        {
            ClearShtora();
            ShtoraVanilla.Visibility = Visibility.Visible;
            if (configuration.ManualFile)
                ShtoraManual.Visibility = Visibility.Visible;
        }

        private void ClearShtora()
        {
            ShtoraBP.Visibility = Visibility.Hidden;
            ShtoraHP.Visibility = Visibility.Hidden;
            ShtoraVanilla.Visibility = Visibility.Hidden;
            ShtoraManual.Visibility = Visibility.Hidden;
        }

        private void QuickStartStart(object sender, RoutedEventArgs e)
        {            
            QuickStartSource.Source = repos.GetImage(true, configuration.Lang, "quickstart");
        }

        private void QuickStartEnd(object sender, RoutedEventArgs e)
        {
            QuickStartSource.Source = repos.GetImage(false, configuration.Lang, "quickstart");
            configuration.QuickStart = !configuration.QuickStart;
            QuickStartIndicatorStatusChange(configuration.QuickStart);
            GetSound2();
            theCode = 0;
        }

        private void ManualFileSelectStart(object sender, RoutedEventArgs e)
        {
            ManualFileSelectSource.Source = repos.GetImage(true, configuration.Lang, "manual");
        }

        private void ManualFileSelectEnd(object sender, RoutedEventArgs e)
        {
            configuration.ManualFile = !configuration.ManualFile;
            ManualFileSelectSource.Source = repos.GetImage(false, configuration.Lang, "manual");

            if (!configuration.ManualFile)
            {                
                AutoUpdateMode();
                configuration.Patch = new Vanilla();
                SetVanillaShtora();
            }
            else
                ManualFileMode();

            GetSound1();
        }

        private void FilesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilesList.SelectedItem != null)
            {
                configuration.PreviousActivatedFiles.Clear();
                configuration.PreviousActivatedFiles.Add(FilesList.SelectedItem.ToString());
            }
            ClearShtora();
            if (configuration.ManualFile)
                ShtoraManual.Visibility = Visibility.Visible;
        }

        private void WindowedStart(object sender, RoutedEventArgs e)
        {            
            WindowedSource.Source = repos.GetImage(true, configuration.Lang, "windowed");
        }

        private void WindowedEnd(object sender, RoutedEventArgs e)
        {
            WindowedSource.Source = repos.GetImage(false, configuration.Lang, "windowed");
            configuration.Windowed = !configuration.Windowed;
            WindowedIndicatorStatusChange(configuration.Windowed);
            GetSound2();
            theCode = 0;
        }

        private void QuickStartIndicatorStatusChange(bool status)
        {
            if (status)
                QSIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_on.png", UriKind.Relative));
            else
                QSIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_off.png", UriKind.Relative));
        }

        private void WindowedIndicatorStatusChange(bool status)
        {
            if (status)
                WindowIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_on.png", UriKind.Relative));
            else
                WindowIndicator.Source = new BitmapImage(new Uri("/Windows/Resources/Main/indicator_off.png", UriKind.Relative));
        }

        private void BPSetStart(object sender, RoutedEventArgs e)
        {            
            BPSource.Source = repos.GetImage(true, configuration.Lang, "BP");
            theCode = 1;
        }

        private void HPSetStart(object sender, RoutedEventArgs e)
        {            
            HPSource.Source = repos.GetImage(true, configuration.Lang, "HP");
            if (theCode == 1)
                theCode = 2;
            else
                theCode = 0;
        }

        private void VanillaSetStart(object sender, RoutedEventArgs e)
        {            
            VanillaSource.Source = repos.GetImage(true, configuration.Lang, "vanilla");
            theCode = 0;
        }

        private void BPSetEnd(object sender, RoutedEventArgs e)
        {
            BPSource.Source = repos.GetImage(false, configuration.Lang, "BP");
            configuration.Patch = new BPatch();
            SetBPShtora();
            GetSound3();
        }

        private void HPSetEnd(object sender, RoutedEventArgs e)
        {
            HPSource.Source = repos.GetImage(false, configuration.Lang, "HP");
            configuration.Patch = new HPatch();
            SetHPShtora();
            GetSound3();
        }

        private void VanillaSetEnd(object sender, RoutedEventArgs e)
        {
            VanillaSource.Source = repos.GetImage(false, configuration.Lang, "vanilla");
            configuration.Patch = new Vanilla();
            if (configuration.ManualFile)
            {
                FilesList.SelectedItem = null;
                configuration.PreviousActivatedFiles.Clear();                
            }

            SetVanillaShtora();
            GetSound3();
        }

        private void OpenHPChangeLogStart(object sender, RoutedEventArgs e)
        {            
            HPChangelogSource.Source = repos.GetImage(true, Lng.Eng, "changelog");
            theCode = 0;
        }

        private void OpenBPChangeLogStart(object sender, RoutedEventArgs e)
        {            
            BPChangelogSource.Source = repos.GetImage(true, Lng.Eng, "changelog");
            theCode = 0;
        }

        private void OpenHPChangeLogEnd(object sender, RoutedEventArgs e)
        {
            HPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            System.Diagnostics.Process.Start(EntryPoint.HPLogURL);
            GetSound2();
        }

        private void OpenBPChangeLogEnd(object sender, RoutedEventArgs e)
        {
            BPChangelogSource.Source = repos.GetImage(false, Lng.Eng, "changelog");
            System.Diagnostics.Process.Start(EntryPoint.BPLogURL);
            GetSound2();
        }

        private void RusSet(object sender, RoutedEventArgs e)
        {           
            configuration.Lang = DeLauncherForm.Language.Rus;
            SetRusLang();
            if (theCode == 2)
                theCode = 3;
            else
                theCode = 0;
        }

        private void EngSet(object sender, RoutedEventArgs e)
        {
            configuration.Lang = DeLauncherForm.Language.Eng;
            SetEngLang();
            theCode = 0;
        }

        private void GoExitStart(object sender, RoutedEventArgs e)
        {
            ExitSource.Source = repos.GetImage(true, configuration.Lang, "exit");
        }

        private void GoExitEnd(object sender, RoutedEventArgs e)
        {
            ExitSource.Source = repos.GetImage(false, configuration.Lang, "exit");
            this.Close();
        }

        #endregion
    }
}
