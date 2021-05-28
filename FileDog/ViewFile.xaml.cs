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
    /// Логика взаимодействия для ViewFile.xaml
    /// </summary>
    public partial class ViewFile : Page
    {
        public ViewFile(Engine.File file)
        {
            InitializeComponent();
            OldPath.Content = file.oldName;
            NewPath.Content = file.newName;
            Date.Content = file.date.ToString();
            string changeType = "";
            switch (file.changeType)
            {
                case Engine.ChangeType.Changed:
                    changeType = "Изменение содержимого";
                    break;
                case Engine.ChangeType.Created:
                    changeType = "Создание";
                    break;
                case Engine.ChangeType.Deleted:
                    changeType = "Удаление";
                    break;
                case Engine.ChangeType.Renamed:
                    changeType = "Переименование";
                    break;
                default:
                    changeType = "Неизвестно";
                    break;
            }
            ChangeType.Content = changeType;
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            Engine.SetPage(Engine.mainPage);
        }
    }
}
