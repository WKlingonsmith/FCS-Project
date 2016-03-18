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
    /// Interaction logic for AddInKindService.xaml
    /// </summary>
    public partial class AddInKindService : Window
    {
        public string ServiceDescription { get; set; }
        public decimal RatePerHour { get; set; }
        public string BeginHour {get; set; }
        public string BeginMinute {get; set; }
        public string EndHour {get; set; }
        public string EndMinute { get; set; }

        public AddInKindService()
        {
            
            RatePerHour = 0.00M;
            InitializeComponent();
        }

        private void Add_InKind_Service(object sender, RoutedEventArgs e)
        {
            if(AMPM_Start.SelectedValue.ToString() == "PM" && Convert.ToInt32(BeginHour) != 12)
            {
                BeginHour = (Convert.ToInt32(BeginHour) + 12).ToString();
            }
            if (AMPM_End.SelectedValue.ToString() == "PM" && Convert.ToInt32(EndHour) != 12)
            {
                EndHour = (Convert.ToInt32(EndHour) + 12).ToString();
            }
            if (AMPM_Start.SelectedValue.ToString() == "AM" && Convert.ToInt32(BeginHour) == 12)
            {
                BeginHour = (Convert.ToInt32(BeginHour) - 12).ToString();
            }
            if (AMPM_End.SelectedValue.ToString() == "AM" && Convert.ToInt32(EndHour) == 12)
            {
                EndHour = (Convert.ToInt32(EndHour) - 12).ToString();
            }

            DateTime help = Convert.ToDateTime(DateRecieved.ToString());
            DateTime startDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(BeginHour), Convert.ToInt32(BeginMinute), 0);
            DateTime endDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(EndHour), Convert.ToInt32(EndMinute), 0);
            decimal timeDiff = (decimal)(endDateTime - startDateTime).TotalHours;
            if (ServiceDescription != null && ServiceDescription != "" && RatePerHour > 0 && timeDiff > 0 && Individual.SelectedIndex != -1)
            {
                string Indiv = Individual.SelectedValue.ToString();
                Models.FCS_FundingContext db = new Models.FCS_FundingContext();
                MessageBox.Show(ServiceDescription + "\n" + RatePerHour + "\n" + startDateTime + "\n" + endDateTime + "\n" + timeDiff + "\n" + Indiv);
                string[] words = Indiv.Split('\t');
                string FName = words[0]; string LName = words[1]; string FNumber = words[2];
                var donorID = (from dc in db.DonorContacts
                               where dc.ContactFirstName == FName && dc.ContactLastName == LName && dc.ContactPhone == FNumber
                               select dc.DonorID).Distinct().FirstOrDefault();

                Models.Donation donation = new Models.Donation(donorID, false, true, 0M, Convert.ToDateTime(DateRecieved.ToString()));
                db.Donations.Add(donation);
                db.SaveChanges();
                Models.In_Kind_Service inKind = new Models.In_Kind_Service(donation.DonationID, startDateTime, endDateTime,
                    RatePerHour, ServiceDescription,  (double)Math.Round(timeDiff, 2), Math.Round(RatePerHour * timeDiff, 2));
                db.In_Kind_Service.Add(inKind);
                db.SaveChanges();
                MessageBox.Show("Successfully added In_Kind Service");
                this.Close();
            }
            else
            {
                MessageBox.Show("Make sure you input correct data.");
            }
        }

        private void Individual_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var query = (from o in db.Donors
                         join c in db.DonorContacts on o.DonorID equals c.DonorID
                         where o.DonorType == "Individual" || o.DonorType == "Anonymous"
                         orderby c.ContactLastName
                         select c.ContactFirstName + "\t" + c.ContactLastName + "\t" + c.ContactPhone).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void AM_PM_Dropdown(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>() { "AM", "PM" };
        }
        

        private void Hour_LostFocus(object sender, RoutedEventArgs e)
        {

            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (value > 12)
                        textbox.Text = "12";
                    else if (value < 1)
                        textbox.Text = "1";
                }
                catch (Exception ex)
                {
                    textbox.Text = "";
                    MessageBox.Show("You inserted a character");
                }
            }

        }

        private void Minute_LostFocus(object sender, RoutedEventArgs e)
        {
            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (textbox.Text.Length == 1)
                    {
                        textbox.Text = "0" + textbox.Text;
                    }
                    else if (value > 59)
                    {
                        textbox.Text = "59";
                    }
                    else if (value < 0)
                    {
                        textbox.Text = "00";
                    }
                }
                catch (Exception ex)
                {
                    textbox.Text = "";
                    MessageBox.Show("You inserted a character");
                }
            }
        }

        private void Hour_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.Text = "12";
        }

        private void Minute_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox.Text = "00";
        }
    }
}
