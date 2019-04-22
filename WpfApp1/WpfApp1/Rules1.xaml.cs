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
    /// Interaction logic for Rules.xaml
    /// </summary>
    public partial class Rules1 : Page
    {

        string passInitials;            //Initials of current player

        //Constructor
        public Rules1(string initials)
        {
            passInitials = initials;
            InitializeComponent();
        }

        //Back button handler
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());                 //Go to main menu
        }

        //Skip Rules button handler
        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Rules3(passInitials));   //Go to last rules page
        }

        //Next button handler
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Rules2(passInitials));   //Go to next rules pages
        }
    }
}