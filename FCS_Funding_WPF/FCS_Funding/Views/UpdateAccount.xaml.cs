using FCS_DataTesting;
//using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateAccount.xaml
    /// </summary>
    public partial class UpdateAccount : Window
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffTitle { get; set; }
        public int StaffID { get; set; }

        public string helperUserName { get; set; }
        public UpdateAccount(AdminDataGrid account)
        {
            UserName = account.StaffUserName;
            FirstName = account.StaffFirstName;
            LastName = account.StaffLastName;
            StaffTitle = account.StaffTitle;
            StaffID = account.StaffID;

            helperUserName = account.StaffUserName;
            InitializeComponent();
        }

        private void Update_Account(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            string Role = UserRole.SelectedValue.ToString();


            int usernameVerify = (from uv in db.Staffs
                                  where uv.StaffUserName == UserName
                                  select uv).Count();
            if (UserName == null || UserName == "" || FirstName == null || FirstName == "" || LastName == null || LastName == "" || StaffTitle == null || StaffTitle == "")
            {
                MessageBox.Show("Make sure you input the correct data");
            }
            else if(usernameVerify != 0 && UserName != helperUserName)
            {
                MessageBox.Show("The username you selected is already taken");
            }
            else
            {
                var staff = (from p in db.Staffs
                              where p.StaffID == StaffID
                              select p).First();
                staff.StaffFirstName = FirstName;
                staff.StaffLastName = LastName;
                staff.StaffTitle = StaffTitle;
                staff.StaffUserName = UserName;
                staff.StaffDBRole = Role;
                db.SaveChanges();
                MessageBox.Show("Updated these changes successfully.");
                this.Close();
            }
        }

        private void UpdatePasword(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingDBModel db = new Models.FCS_FundingDBModel();
            string password = Password.Password.ToString();
            string verifiedPW = VerifyPassword.Password.ToString();
            string hashedPassword = PasswordHashing.GetHashString(password);

            if (password.Length < 5)
            {
                MessageBox.Show("Please pick a longer password (Minimum length of 5)");
            }
            else if (password != verifiedPW)
            {
                MessageBox.Show("Your Passwords do not match!");
            }
            else
            {
                var staff = (from p in db.Staffs
                             where p.StaffID == StaffID
                             select p).First();
                staff.StaffPassword = hashedPassword;
                db.SaveChanges();
                MessageBox.Show("Updated these changes successfully.");
                this.Close();
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
