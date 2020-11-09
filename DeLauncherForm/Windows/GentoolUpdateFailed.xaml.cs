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
    /// Логика взаимодействия для GentoolUpdateFailed.xaml
    /// </summary>
    public partial class GentoolUpdateFailed : Window
    { 
        public GentoolUpdateFailed(FormConfiguration cfg)
        {
            InitializeComponent();
            if (cfg.Lang == DeLauncherForm.Language.Rus)
                ErrorMessage1.Text = "Обновление gentool неуспешно, продолжаем запуск с текущей версией";

            Ok.Click += CloseManually;            
        }

        private void CloseManually(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
