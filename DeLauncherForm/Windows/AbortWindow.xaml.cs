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
    /// Логика взаимодействия для AbortWindow.xaml
    /// </summary>
    public partial class AbortWindow : Window
    {
        public AbortWindow(FormConfiguration conf)
        {
            InitializeComponent();
            Ok.Click += CloseWindow;

            if (conf.Lang == DeLauncherForm.Language.Rus)
            {
                ErrorMessage1.Text = "Другой процесс DeLauncher уже(или ещё) активен!";
                ErrorMessage2.Text = "Пожалуйста, подождите и перезапустите DeLauncher";
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
