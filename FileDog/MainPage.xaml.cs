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

namespace FileDog
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public void PastePath(Engine.File file)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(8, GridUnitType.Star)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(2, GridUnitType.Star)
            });
            Viewbox viewbox = new Viewbox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(15),
                MinHeight = 30,
                MaxHeight = 40
            };
            Grid.SetColumn(viewbox, 0);
            Label label = new Label()
            {
                Foreground = Brushes.White,
                Cursor = Cursors.Hand,
                Tag = file
            };
            if (file.newName != null)
            {
                label.Content = file.oldName.Split('\\').Last()
                    + " -> " + file.newName.Split('\\').Last();
            }
            else label.Content = file.oldName.Split('\\').Last();
            label.MouseDown += FileInfo;
            viewbox.Child = label;
            
            Viewbox datebox = new Viewbox()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(15),
                MinHeight = 30,
                MaxHeight = 40
            };
            Grid.SetColumn(datebox, 1);
            Label date = new Label()
            {
                Foreground = Brushes.White,
                Content = file.date.ToString()
            };
            datebox.Child = date;

            grid.Children.Add(viewbox);
            grid.Children.Add(datebox);
            Panel.Children.Add(grid);
        }

        private void FileInfo(object sender, MouseButtonEventArgs e)
        {
            Engine.SetPage(new ViewFile((sender as Label).Tag
                as Engine.File));
        }

        private void OpenOptions(object sender, MouseButtonEventArgs e)
        {
            Engine.SetPage(new Options.Main(Engine.state));
        }
    }
}
