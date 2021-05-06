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

namespace FileDog.FirstStart
{
    /// <summary>
    /// Логика взаимодействия для Paths.xaml
    /// </summary>
    public partial class Paths : Page
    {
        public Engine.Database state;
        public Paths(Engine.Database database)
        {
            state = database;
            InitializeComponent();
        }

        private void AddPath(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                Engine.firstStartStack.NextPage(this, 
                    new EditPath(path));
            }
        }

        public void UpdatePanel(EditPath dialog, Label sender = null)
        {
            if (dialog.result == EditPath.Result.Save && 
                sender == null)
            {
                PastePath(dialog.path);
            }
            else if (dialog.result == EditPath.Result.Delete &&
                sender != null)
            {
                Panel.Children.Remove(sender);
            }
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
                Content = path
            };
            viewbox.Child = label;
            Panel.Children.Add(viewbox);
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            Engine.firstStartStack.PreviousPage();
        }
    }
}
