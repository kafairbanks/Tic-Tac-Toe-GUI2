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
    /// Interaction logic for Initials.xaml
    /// </summary>
    public partial class Initials : Page
    {
        public Initials()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            string input = initialsInput.Text;
            if (input.Length == 3)
            {
                input = input.ToUpper();
                NavigationService.Navigate(new Rules1(input));
            }
            else
            {
                initialsInput.Text = "";
                warningText.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
