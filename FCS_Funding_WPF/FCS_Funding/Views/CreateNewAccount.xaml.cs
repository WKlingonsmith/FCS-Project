using FCS_DataTesting;
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
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffTitle { get; set; }

        public CreateNewAccount()
        {
            InitializeComponent();
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            string Role = UserRole.SelectedValue.ToString();

            string password = Password.Password.ToString();
            string verifiedPW = VerifyPassword.Password.ToString();
            string hashedPassword = PasswordHashing.GetHashString(password);

            int usernameVerify = (from uv in db.Staffs
                                  where uv.StaffUserName == UserName
                                  select uv).Count();
            if (Role == "No Access")
            {
                if (FirstName == null || FirstName == "" || LastName == null || LastName == "" || StaffTitle == null || StaffTitle == "")
                {
                    MessageBox.Show("Make sure you input the correct data");
                }
                else
                {
                    Models.Staff account = new Models.Staff();
                    account.StaffFirstName = FirstName;
                    account.StaffLastName = LastName;
                    account.StaffTitle = StaffTitle;
                    account.StaffUserName = UserName;
                    account.StaffPassword = hashedPassword;
                    account.StaffDBRole = Role;
                    db.Staffs.Add(account);
                    db.SaveChanges();
                    this.Close();
                }
            }
            else
            {
                if (password.Length < 5)
                {
                    MessageBox.Show("Please pick a longer password (Minimum length of 5)");
                }
                else if (password != verifiedPW)
                {
                    MessageBox.Show("Your Passwords do not match!");
                }
                else if (UserName == null || UserName == "" || FirstName == null || FirstName == ""
                    || LastName == null || LastName == "" || StaffTitle == null || StaffTitle == "")
                {
                    MessageBox.Show("Make sure you input the correct data");
                }
                else if(usernameVerify != 0)
                {
                    MessageBox.Show("The username you selected is already taken");
                }
                else
                {
                    Models.Staff account = new Models.Staff();
                    account.StaffFirstName = FirstName;
                    account.StaffLastName = LastName;
                    account.StaffTitle = StaffTitle;
                    account.StaffUserName = UserName;
                    account.StaffPassword = hashedPassword;
                    account.StaffDBRole = Role;
                    db.Staffs.Add(account);
                    db.SaveChanges();
                    this.Close();
                }
            }
        }

        private void UserType_DropdownLoaded(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>()
            {
                "No Access", //User cannot login
                "Basic",    //Read
                "User",     //Read, Insert, Update
                "Admin"     //Read, Insert, Update, Delete
            };
        }
    }
}
