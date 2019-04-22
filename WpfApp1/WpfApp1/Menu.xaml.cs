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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()

        {
            InitializeComponent();
        }

        private void hsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HighScores());
        }

        private void gameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Initials());
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit?", "Quit Game", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No: break;
            }
        }
    }

}
