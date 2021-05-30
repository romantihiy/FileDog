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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace Starter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string masterpath = masterpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Local\\FileIndexer\\View.exe";
            masterpath = masterpath.Replace("\\Roaming", "");
            if (File.Exists(masterpath))
            {
                Process view = new Process();
                view.StartInfo.FileName = masterpath;
                view.StartInfo.WorkingDirectory = masterpath.Replace("\\View.exe", "");
                view.Start();
                Close();
            }
            else
            {
                MessageBox.Show("Файл не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }
}
