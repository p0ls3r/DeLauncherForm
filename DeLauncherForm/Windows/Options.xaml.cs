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
        private ImageHandler repos;
        public MediaPlayer player1 = new MediaPlayer();
        public MediaPlayer player2 = new MediaPlayer();
        public MediaPlayer player3 = new MediaPlayer();


        public Options(FormConfiguration cfg, LaunchOptions opt, ImageHandler rep)
        {
            InitializeComponent();
            configuration = cfg;
            options = opt;
            repos = rep;

            SetButtonsImages();

            if (configuration.Lang == DeLauncherForm.Language.Rus)
                SetRus();
            else
                SetEng();

            SetOptions();
            SetButtonsBindings();
            GetSound1();
        }

        private void StopPlayers()
        {
            if (options.Sounds)
            {
                player1.Close();
                player2.Close();
                player3.Close();
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
                SetVolume();
                player3.Open(new Uri(EntryPoint.LauncherFolder + "press1_new.wav", UriKind.Relative));
                player3.Play();
            }
        }

        private void GetSound2()
        {
            if (options.Sounds)
            {
                StopPlayers();
                SetVolume();
                player1.Open(new Uri(EntryPoint.LauncherFolder + "press2.wav", UriKind.Relative));               
                player1.Play();
            }
        }

        private void GetSound3()
        {
            if (options.Sounds)
            {
                StopPlayers();
                SetVolume();
                player2.Open(new Uri(EntryPoint.LauncherFolder + "press3.wav", UriKind.Relative));                
                player2.Play();
            }
        }

        private void ClosePlayers(object sender, EventArgs e)
        {
            player1.Volume = 0;
            player2.Volume = 0;
            player3.Volume = 0;
            StopPlayers();
        }

        public void Close(object sender, EventArgs e)
        {
            CloseWindow();
        }

        private void SetRus()
        {
            text.Source = new BitmapImage(new Uri("/Windows/Resources/Options/text_r.png", UriKind.Relative));
            help.Source = new BitmapImage(new Uri("/Windows/Resources/Options/help_r.png", UriKind.Relative));                       
        }

        private void SetEng()
        {
            text.Source = new BitmapImage(new Uri("/Windows/Resources/Options/text_e.png", UriKind.Relative));
            help.Source = new BitmapImage(new Uri("/Windows/Resources/Options/help_e.png", UriKind.Relative));
        }

        private void SetButtonsImages()
        {            
            moddedSource.Source = repos.GetImage(false, configuration.Lang, "modded");
            originalSource.Source = repos.GetImage(false, configuration.Lang, "generals");

            AutoUpdateSource.Source = repos.GetImage(false, configuration.Lang, "latestversion");
            CurrentVersionSource.Source = repos.GetImage(false, configuration.Lang, "currentversion");
            RemoveSource.Source = repos.GetImage(false, configuration.Lang, "disable");

            fixfileSource.Source = repos.GetImage(false, configuration.Lang, "fixfile");
            ApplySource.Source = repos.GetImage(false, configuration.Lang, "apply");

            DeleteOldSource.Source = repos.GetImage(false, configuration.Lang, "deleteold");
            soundsSource.Source = repos.GetImage(false, configuration.Lang, "sounds");
        }

        private void SetOptions()
        {
            if (options.ModdedExe)
                SetModdedShtora();
            else
                SetGeneralsShtora();

            SetIndicator1();
            SetIndicator2();
            SetIndicator3();

            if (options.Gentool == GentoolsMode.AutoUpdate)
                SetAutoUpdateShtora();
            if (options.Gentool == GentoolsMode.Current)
                SetCurrentVersionShtora();
            if (options.Gentool == GentoolsMode.Disable)
                SetRemoveShtora();
            
        }
        private void SetButtonsBindings()
        {
            modded.PreviewMouseLeftButtonDown += ModdedSetStart;
            modded.PreviewMouseLeftButtonUp += ModdedSetEnd;
            original.PreviewMouseLeftButtonDown += OriginalSetStart;
            original.PreviewMouseLeftButtonUp += OriginalSetEnd;
            fixfile.PreviewMouseLeftButtonDown += FixFileSetStart;
            fixfile.PreviewMouseLeftButtonUp += FixFileSetEnd;
            AutoUpdate.PreviewMouseLeftButtonDown += AutoUpdateSetStart;
            AutoUpdate.PreviewMouseLeftButtonUp += AutoUpdateSetEnd;
            CurrentVersion.PreviewMouseLeftButtonDown += CurrentVersionSetStart;
            CurrentVersion.PreviewMouseLeftButtonUp += CurrentVersionSetEnd;
            Remove.PreviewMouseLeftButtonDown += RemoveSetStart;
            Remove.PreviewMouseLeftButtonUp += RemoveSetEnd;
            Apply.PreviewMouseLeftButtonDown += ApplyingStart;
            Apply.PreviewMouseLeftButtonUp += ApplyingEnd;
            DeleteOld.PreviewMouseLeftButtonDown += DeleteOldSetStart;
            DeleteOld.PreviewMouseLeftButtonUp += DeleteOldSetEnd;

            sounds.PreviewMouseLeftButtonDown += SoundSetStart;
            sounds.PreviewMouseLeftButtonUp += SoundSetEnd;


            this.MouseDown += Window_MouseDown;            
            this.Closing += Close;
            this.Unloaded += ClosePlayers;
        }

        private void SoundSetStart(object sender, EventArgs e)
        {            
            soundsSource.Source = repos.GetImage(true, configuration.Lang, "sounds");
        }

        private void SoundSetEnd(object sender, EventArgs e)
        {
            soundsSource.Source = repos.GetImage(false, configuration.Lang, "sounds");            
            options.Sounds = !options.Sounds;
            GetSound2();
            SetIndicator3();
        }

        private void SetIndicator3()
        {
            if (options.Sounds)
                indicator3.Source = new BitmapImage(new Uri("/Windows/Resources/Options/indicator_on.png", UriKind.Relative));
            else
                indicator3.Source = new BitmapImage(new Uri("/Windows/Resources/Options/indicator_off.png", UriKind.Relative));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void ModdedSetStart(object sender, EventArgs e)
        {            
            GetSound3();
            moddedSource.Source = repos.GetImage(true, configuration.Lang, "modded");
        }

        private void ModdedSetEnd(object sender, EventArgs e)
        {
            moddedSource.Source = repos.GetImage(false, configuration.Lang, "modded");
            options.ModdedExe = true;
            SetModdedShtora();
        }

        private void OriginalSetStart(object sender, EventArgs e)
        {
            GetSound3();
            originalSource.Source = repos.GetImage(true, configuration.Lang, "generals");
        }

        private void OriginalSetEnd(object sender, EventArgs e)
        {
            originalSource.Source = repos.GetImage(false, configuration.Lang, "generals");
            options.ModdedExe = false;
            SetGeneralsShtora();
        }

        private void SetGeneralsShtora()
        {
            ClearShtorasFiles();
            shtora2.Visibility = Visibility.Visible;
        }

        private void SetModdedShtora()
        {
            ClearShtorasFiles();
            shtora1.Visibility = Visibility.Visible;
        }

        private void ClearShtorasFiles()
        {
            shtora1.Visibility = Visibility.Hidden;
            shtora2.Visibility = Visibility.Hidden;
        }

        private void FixFileSetStart(object sender, EventArgs e)
        {
            GetSound2();
            fixfileSource.Source = repos.GetImage(true, configuration.Lang, "fixfile");
        }

        private void FixFileSetEnd(object sender, EventArgs e)
        {
            fixfileSource.Source = repos.GetImage(false, configuration.Lang, "fixfile");
            options.FixFile = !options.FixFile;
            SetIndicator1();
        }

        private void SetIndicator1()
        {
            if (options.FixFile)
                indicator1.Source = new BitmapImage(new Uri("/Windows/Resources/Options/indicator_on.png", UriKind.Relative));
            else
                indicator1.Source = new BitmapImage(new Uri("/Windows/Resources/Options/indicator_off.png", UriKind.Relative));
        }

        private void AutoUpdateSetStart(object sender, EventArgs e)
        {
            GetSound3();
            AutoUpdateSource.Source = repos.GetImage(true, configuration.Lang, "latestversion");
        }

        private void AutoUpdateSetEnd(object sender, EventArgs e)
        {
            AutoUpdateSource.Source = repos.GetImage(false, configuration.Lang, "latestversion");
            options.Gentool = GentoolsMode.AutoUpdate;
            SetAutoUpdateShtora();
        }

        private void CurrentVersionSetStart(object sender, EventArgs e)
        {
            GetSound3();
            CurrentVersionSource.Source = repos.GetImage(true, configuration.Lang, "currentversion");
        }

        private void CurrentVersionSetEnd(object sender, EventArgs e)
        {
            CurrentVersionSource.Source = repos.GetImage(false, configuration.Lang, "currentversion");
            options.Gentool = GentoolsMode.Current;
            SetCurrentVersionShtora();
        }

        private void RemoveSetStart(object sender, EventArgs e)
        {
            GetSound3();
            RemoveSource.Source = repos.GetImage(true, configuration.Lang, "disable");
        }

        private void RemoveSetEnd(object sender, EventArgs e)
        {
            RemoveSource.Source = repos.GetImage(false, configuration.Lang, "disable");
            options.Gentool = GentoolsMode.Disable;
            SetRemoveShtora();
        }

        private void SetAutoUpdateShtora()
        {
            ClearShtorasGentools();
            shtora3.Visibility = Visibility.Visible;
        }

        private void SetCurrentVersionShtora()
        {
            ClearShtorasGentools();
            shtora4.Visibility = Visibility.Visible;
        }

        private void SetRemoveShtora()
        {
            ClearShtorasGentools();
            shtora5.Visibility = Visibility.Visible;
        }

        private void ClearShtorasGentools()
        {
            shtora3.Visibility = Visibility.Hidden;
            shtora4.Visibility = Visibility.Hidden;
            shtora5.Visibility = Visibility.Hidden;
        }

        private void ApplyingStart(object sender, EventArgs e)
        {
            ApplySource.Source = repos.GetImage(true, configuration.Lang, "apply");
        }

        private void ApplyingEnd(object sender, EventArgs e)
        {
            //StopPlayers();
            ApplySource.Source = repos.GetImage(false, configuration.Lang, "apply");
            ApplyOptions(configuration, options);
            this.Close();
        }

        private void DeleteOldSetStart(object sender, EventArgs e)
        {
            GetSound2();
            DeleteOldSource.Source = repos.GetImage(true, configuration.Lang, "deleteold");
        }

        private void DeleteOldSetEnd(object sender, EventArgs e)
        {
            DeleteOldSource.Source = repos.GetImage(false, configuration.Lang, "deleteold");
            options.DeleteOldVersions = !options.DeleteOldVersions;
            SetIndicator2();
        }

        private void SetIndicator2()
        {
            if (options.DeleteOldVersions)
                indicator2.Source = new BitmapImage(new Uri("/Windows/Resources/Options/indicator_on.png", UriKind.Relative));
            else
                indicator2.Source = new BitmapImage(new Uri("/Windows/Resources/Options/indicator_off.png", UriKind.Relative));
        }

    }
}
