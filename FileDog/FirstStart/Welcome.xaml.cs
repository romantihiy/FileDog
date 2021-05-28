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

namespace FileDog.FirstStart
{
    /// <summary>
    /// Логика взаимодействия для Welcome.xaml
    /// </summary>
    public partial class Welcome : Page
    {
        public Engine.Database state;
        public Welcome(Engine.Database database)
        {
            state = database;
            InitializeComponent();
        }

        public bool passVis = false;
        private void ShowPass(object sender, MouseButtonEventArgs e)
        {
            if (passVis)
            {
                this.PassBox.Password = this.TextBox.Text;
                this.TextBox.Visibility = Visibility.Hidden;
                this.PassBox.Visibility = Visibility.Visible;
                this.ShowButton.Content = "Показать пароль";
                passVis = false;
            }
            else
            {
                this.TextBox.Text = this.PassBox.Password;
                this.PassBox.Visibility = Visibility.Hidden;
                this.TextBox.Visibility = Visibility.Visible;
                this.ShowButton.Content = "Скрыть пароль";
                passVis = true;
            }
        }

        private void Next(object sender, MouseButtonEventArgs e)
        {
            state.settings.Changed = this.Changed.IsChecked.Value;
            state.settings.Created = this.Created.IsChecked.Value;
            state.settings.Deleted = this.Deleted.IsChecked.Value;
            state.settings.Renamed = this.Renamed.IsChecked.Value;
            state.settings.IncludeSubfolders = this.Subfolders.IsChecked.Value;
            if (passVis)
            {
                state.settings.pass = Engine.GetHash(
                    this.TextBox.Text);
            }
            else
            {
                state.settings.pass = Engine.GetHash(
                    this.PassBox.Password);
            }
            Engine.firstStartStack.NextPage(this);
        }
    }
}
