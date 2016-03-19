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

namespace FCS_Funding
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method logs you into the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            string pw = Password.Password.ToString();
            string us = Username.Text;
            string hashedPassword = FCS_DataTesting.PasswordHashing.GetHashString(pw);
            try
            {
                var query = (from p in db.Staffs
                             where p.StaffUserName == us && p.StaffPassword == hashedPassword
                             select p).Distinct().First();
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect Credentials");
            }
            
        }
    }
}
