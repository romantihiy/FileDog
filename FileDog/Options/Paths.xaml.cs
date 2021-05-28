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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileDog.Options
{
    /// <summary>
    /// Логика взаимодействия для Paths.xaml
    /// </summary>
    public partial class Paths : Page
    {
        public Engine.Database database;
        public List<string> paths;
        public Paths(Engine.Database _database)
        {
            InitializeComponent();
            database = _database;
            paths = new List<string>();
            foreach (string path in database.paths)
                PastePath(path);
        }
        public void PastePath(string path)
        {
            Viewbox viewbox = new Viewbox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(15),
                MinHeight = 30,
                MaxHeight = 40
            };
            Label label = new Label()
            {
                Foreground = Brushes.White,
                Content = path,
                Cursor = Cursors.Hand
            };
            viewbox.MouseDown += DeletePath;
            viewbox.Child = label;
            Panel.Children.Add(viewbox);
            paths.Add(path);
        }

        private void DeletePath(object sender, MouseButtonEventArgs e)
        {
            var dialogResult = MessageBox.Show("Удалить?", "Предупреждение", 
                MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                paths.Remove(((sender as Viewbox).Child as Label).
                    Content.ToString());
                Panel.Children.Remove(sender as Viewbox);
            }
        }
        private void AddPath(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (!paths.Contains(dialog.FileName))
                    PastePath(dialog.FileName);
            }
        }
    }
}
