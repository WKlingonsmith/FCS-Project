using FCS_DataTesting;
using System;
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
    /// Interaction logic for UpdateInKindService.xaml
    /// </summary>
    public partial class UpdateInKindService : Window
    {
        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int ServiceID { get; set; }
        //properties
        public string ServiceDescription { get; set; }
        public decimal RatePerHour { get; set; }
        public string BeginHour { get; set; }
        public string BeginMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public UpdateInKindService(InKindService p)
        {
            DonorID = p.DonorID;
            DonationID = p.DonationID;
            ServiceID = p.ServiceID;
            ServiceDescription = p.ServiceDescription;
            RatePerHour = Math.Round(p.RatePerHour, 2);
            BeginMinute = p.StartDateTime.Minute.ToString();
            EndMinute = p.EndDateTime.Minute.ToString();

            if (BeginMinute.Length == 1) 
            {
                BeginMinute = "0" + BeginMinute; 
            }
            if (EndMinute.Length == 1) 
            {
                EndMinute = "0" + EndMinute; 
            }
            if (p.StartDateTime.Hour > 12)  
            {
                BeginHour = (p.StartDateTime.Hour - 12).ToString();  
            }
            else if (p.StartDateTime.Hour == 0)
            {
                BeginHour = "12"; 
            }
            else 
            {
                BeginHour = p.StartDateTime.Hour.ToString(); 
            }
            if(p.EndDateTime.Hour > 12)
            {
                EndHour = (p.EndDateTime.Hour - 12).ToString();
            }
            else if(p.EndDateTime.Hour == 0) 
            {
                EndHour = "12";
            }
            else
            {
                EndHour = p.EndDateTime.Hour.ToString();
            }
            InitializeComponent();
			text_ServiceDescription.Focus();
        }

        private void Update_InKind_Service(object sender, RoutedEventArgs e)
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
            try
            {
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                //MessageBox.Show(ServiceDescription + "\n" + RatePerHour + "\n" + startDateTime + "\n" + endDateTime + "\n" + timeDiff );

                var inkindservice = (from p in db.In_Kind_Service
                                  where p.ServiceID == ServiceID
                                  select p).First();
                inkindservice.ServiceDescription = ServiceDescription;
                inkindservice.RatePerHour = RatePerHour;
                inkindservice.StartDateTime = startDateTime;
                inkindservice.EndDateTime = endDateTime;
                inkindservice.ServiceValue = Math.Round(RatePerHour * timeDiff, 2);
                inkindservice.ServiceLength = (double)Math.Round(timeDiff, 2);


                var donation2 = (from d in db.Donations
                                where d.DonationID == DonationID
                                select d).First();
                donation2.DonationDate = Convert.ToDateTime(DateRecieved.ToString());

                db.SaveChanges();
                MessageBox.Show("Successfully updated In_Kind Service");
                this.Close();

            }
            catch
            {
                MessageBox.Show("Please check the data entered.");
            }
        }

        private void Delete_InKind_Service(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this In-Kind Service?",
                 "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try {
                    FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
                    var inkindservice = (from p in db.In_Kind_Service
                                         where p.ServiceID == ServiceID
                                         select p).First();

                    var donation = (from d in db.Donations
                                    where d.DonationID == DonationID
                                    select d).First();
                    db.In_Kind_Service.Remove(inkindservice);
                    db.Donations.Remove(donation);
                    db.SaveChanges();
                    MessageBox.Show("This grant has been deleted and the Proposal associated\nwith this grant has been set to Pending.");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Something went wrong. Please try again.");
                }
            }
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
                catch 
                {
                    textbox.Text = "";
                    MessageBox.Show("Please insert a number.");
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
                catch 
                {
                    textbox.Text = "";
                    MessageBox.Show("Please insert a number.");
                }
            }
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
