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
using FCS_Funding.Models;
using System.Xml.Linq;

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
        FCS_DBModel db = new FCS_DBModel();
        public UpdateAccount(AdminDataGrid account)
        {
            UserName = account.StaffUserName;
            FirstName = account.StaffFirstName;
            LastName = account.StaffLastName;
            StaffTitle = account.StaffTitle;
            StaffID = account.StaffID;

            helperUserName = account.StaffUserName;
            InitializeComponent();
			text_UserName.Focus();
        }

        private void Update_Account(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel();            
            try
            {
                string Role = UserRole.SelectedValue.ToString();
                int usernameVerify = (from uv in db.Staff
                                      where uv.StaffUserName == UserName
                                      select uv).Count();
                if (usernameVerify != 0 && UserName != helperUserName)
                {
                    MessageBox.Show("The username selected is already taken");
                }
                else
                {
                    try
                    {
                        var staff = (from p in db.Staff
                                     where p.StaffID == StaffID
                                     select p).First();
                        staff.StaffFirstName = FirstName;
                        staff.StaffLastName = LastName;
                        staff.StaffTitle = StaffTitle;
                        staff.StaffUserName = UserName;
                        staff.StaffDBRole = Role;
                        db.SaveChanges();
                        
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Please check the data entered.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please check the data entered.");
            }
        }

        private void UpdatePasword(object sender, RoutedEventArgs e)
        {
            db = new FCS_DBModel();
            string password = Password.Password.ToString();
            string verifiedPW = VerifyPassword.Password.ToString();
            string hashedPassword = PasswordHashing.GetHashString(password);

            if (password.Length < 5)
            {
                MessageBox.Show("Please pick a longer password (Minimum length of 5)");
            }
            else if (password != verifiedPW)
            {
                MessageBox.Show("Passwords do not match!");
            }
            else
            {
                try {
                    var staff = (from p in db.Staff
                                 where p.StaffID == StaffID
                                 select p).First();
                    staff.StaffPassword = hashedPassword;
                    db.SaveChanges();
                    
                }
                catch
                {
                    MessageBox.Show("Something went wrong. Please try again.");
                }
                this.Close();
            }
        }
        private void UserType_DropdownLoaded(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>()
            {
				Definition.Definition.NoAccess,
				Definition.Definition.FrontDesk,
				Definition.Definition.User,
				Definition.Definition.Admin
            };
        }

        private void DeleteAccount(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Account?",
                 "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            
                db = new FCS_DBModel();
                var staff = (from s in db.Staff
                             where s.StaffID == StaffID
                             select s).First();

                db.Staff.Remove(staff);
                db.SaveChanges();
            
            this.Close();
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
