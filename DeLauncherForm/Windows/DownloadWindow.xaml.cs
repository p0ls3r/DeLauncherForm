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

namespace DeLauncherForm
{
    /// <summary>
    /// Логика взаимодействия для DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        private FormConfiguration configuration;
        public DownloadWindow(FormConfiguration conf)
        {
            InitializeComponent();

            configuration = conf;

            if (conf.Lang == DeLauncherForm.Language.Rus)
            {
                Update1.Text = "Обновление уже в пути!";
                Update2.Text = "Даже взяли чинук! Терпение)";
            }
        }

        public void UpdateInformation(int percent)
        {
            if (configuration.Lang == DeLauncherForm.Language.Rus)
            {
                DownloadInfo.Text = "Скачивание файла: " + ReposWorker.CurrentFileName;
            }
            else
            {
                DownloadInfo.Text = "Loading file: " + ReposWorker.CurrentFileName;
            }
            ProgressBar.Value = percent;
        }
    }
}
