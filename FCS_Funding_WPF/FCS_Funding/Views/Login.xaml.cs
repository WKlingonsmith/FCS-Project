
using System.Linq;
using System.Windows;

namespace FCS_Funding
{
	using Definition;

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
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            string pw = Password.Password.ToString();
            string us = Username.Text;
            string hashedPassword = FCS_DataTesting.PasswordHashing.GetHashString(pw);
            try
            {
                var query = (from p in db.Staff
                             where p.StaffUserName == us && p.StaffPassword == hashedPassword
                             select p).Distinct().First();
                MainWindow mw = new MainWindow(query.StaffDBRole, query.StaffFirstName + " " + query.StaffLastName);
                if(query.StaffDBRole == Definition.NoAccess)
                {
                    MessageBox.Show("Invalid Credentials");
                    mw.Close();
                    return;
                }
                else if(query.StaffDBRole == Definition.Basic || query.StaffDBRole == Definition.User)
                {
                    mw.AdminTab.IsEnabled = false;
                    mw.AdminTab.Visibility = Visibility.Collapsed;
                }
                else if (query.StaffDBRole == Definition.Admin)
                {
                    mw.AdminTab.IsEnabled = true;
                    mw.AdminTab.Visibility = Visibility.Visible;
                }
                mw.Show();
                this.Close();
            }
            catch 
            {
                MessageBox.Show("The credentials used are either invalid,or\nsomeone else is currently logged in.");
            }
            
        }

        private void ThisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Username.Focus();
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
