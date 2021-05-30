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
using Microsoft.Win32;
using System.Diagnostics;

namespace FileDogInstaller
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string masterpath;
        public MainWindow()
        {
            InitializeComponent();
            masterpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Local\\FileIndexer";
            masterpath = masterpath.Replace("\\Roaming", "");
        }

        private void Install(object sender, MouseButtonEventArgs e)
        {
            if (Directory.Exists(masterpath))
                MessageBox.Show("FileDog уже установлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Directory.CreateDirectory(masterpath);
                using (FileStream fs = new FileStream(masterpath + "\\FileIndexer.exe", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.FileIndexer, 0, Properties.Resources.FileIndexer.Length);
                }
                using (FileStream fs = new FileStream(masterpath + "\\Microsoft.WindowsAPICodePack.dll", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.Microsoft_WindowsAPICodePack, 0, Properties.Resources.Microsoft_WindowsAPICodePack.Length);
                }
                using (FileStream fs = new FileStream(masterpath + "\\Microsoft.WindowsAPICodePack.Shell.dll", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.Microsoft_WindowsAPICodePack_Shell, 0, Properties.Resources.Microsoft_WindowsAPICodePack_Shell.Length);
                }
                using (FileStream fs = new FileStream(masterpath + "\\Microsoft.WindowsAPICodePack.ShellExtensions.dll", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.Microsoft_WindowsAPICodePack_ShellExtensions, 0, Properties.Resources.Microsoft_WindowsAPICodePack_ShellExtensions.Length);
                }
                using (FileStream fs = new FileStream(masterpath + "\\View.exe", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.View, 0, Properties.Resources.View.Length);
                }
                using (FileStream fs = new FileStream(masterpath + "\\FileIndexerStarter.exe", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.FileIndexerStarter, 0, Properties.Resources.FileIndexerStarter.Length);
                }
                using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\FileDog.exe", FileMode.Create, FileAccess.Write))
                {
                    fs.Write(Properties.Resources.FileDog, 0, Properties.Resources.FileDog.Length);
                }

                var key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                key.SetValue("FileIndexer", masterpath + "\\FileIndexerStarter.exe");
                key.Close();
                MessageBox.Show("Успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Delete(object sender, MouseButtonEventArgs e)
        {
            if (!Directory.Exists(masterpath))
                MessageBox.Show("FileDog не установлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                var processes = Process.GetProcessesByName("FileIndexer");
                foreach (var proc in processes)
                {
                    proc.Kill();
                    proc.WaitForExit();
                }
                try
                {
                    Directory.Delete(masterpath, recursive: true);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Закройте FileDog, пожалуйста", "Удаление не завершено", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                try
                {
                    var key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                    key.DeleteValue("FileIndexer");
                    key.Close();
                }
                catch { }
                MessageBox.Show("Успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Reset(object sender, MouseButtonEventArgs e)
        {
            if (!Directory.Exists(masterpath))
                MessageBox.Show("FileDog не установлен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                MessageBoxResult res = MessageBox.Show("История зафиксированных изменений будет удалена", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.OK)
                {
                    if (File.Exists(masterpath + "\\data.xml"))
                        File.Delete(masterpath + "\\data.xml");
                    else
                        MessageBox.Show("База данных отсутствует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
