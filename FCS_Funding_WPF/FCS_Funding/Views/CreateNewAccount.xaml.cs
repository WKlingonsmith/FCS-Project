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
using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateNewAccount.xaml
    /// </summary>
    public partial class CreateNewAccount : Window
    {
        public CreateNewAccount()
        {
            InitializeComponent();
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            string pw = Password.Password.ToString();
            MessageBox.Show(pw);
        }

        private void UserType_DropdownLoaded(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>() { "Admin", "User", "Basic", "No Access" };
        }
    }
}
