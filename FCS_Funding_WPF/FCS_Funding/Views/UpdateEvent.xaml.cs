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
    /// Interaction logic for UpdateEvent.xaml
    /// </summary>
    public partial class UpdateEvent : Window
    {
        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int EventID { get; set; }

        public string BeginHour { get; set; }
        public string BeginMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public UpdateEvent(EventsDataGrid p)
        {
            EventID = p.EventID;
            EventName = p.EventName;
            EventDescription = p.EventDescription;
            BeginMinute = p.EventStartDateTime.Minute.ToString();
            EndMinute = p.EventEndDateTime.Minute.ToString();

            if (BeginMinute.Length == 1)
            {
                BeginMinute = "0" + BeginMinute;
            }
            if (EndMinute.Length == 1)
            {
                EndMinute = "0" + EndMinute;
            }
            if (p.EventStartDateTime.Hour > 12)
            {
                BeginHour = (p.EventStartDateTime.Hour - 12).ToString();
            }
            else if (p.EventStartDateTime.Hour == 0)
            {
                BeginHour = "12";
            }
            else
            {
                BeginHour = p.EventStartDateTime.Hour.ToString();
            }
            if (p.EventEndDateTime.Hour > 12)
            {
                EndHour = (p.EventEndDateTime.Hour - 12).ToString();
            }
            else if (p.EventEndDateTime.Hour == 0)
            {
                EndHour = "12";
            }
            else
            {
                EndHour = p.EventEndDateTime.Hour.ToString();
            }
            InitializeComponent();
        }

        private void Update_Event(object sender, RoutedEventArgs e)
        {
            if (AMPM_Start.SelectedValue.ToString() == "PM" && Convert.ToInt32(BeginHour) != 12)
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
            if (EventName != null && EventName != "" && EventDescription != null && EventDescription != "" && timeDiff > 0)
            {
                Models.FCS_FundingContext db = new Models.FCS_FundingContext();
                MessageBox.Show(EventName + "\n" + EventDescription + "\n" + startDateTime + "\n" + endDateTime + "\n" + timeDiff);

                var event1 = (from p in db.FundRaisingEvents
                                     where p.EventID == EventID
                                     select p).First();
                event1.EventName = EventName;
                event1.EventDescription = EventDescription;
                event1.EventStartDateTime = startDateTime;
                event1.EventEndDateTime = endDateTime;
                                
                db.SaveChanges();
                MessageBox.Show("Successfully updated Event");
                this.Close();

            }
            else
            {
                MessageBox.Show("Make sure you input correct data.");
            }
        }

        private void Delete_Event(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Confirmation",
                "Are you sure that you want to delete this In-Kind Service?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_FundingContext db = new FCS_Funding.Models.FCS_FundingContext();
                var event1 = (from d in db.FundRaisingEvents
                                where d.EventID == EventID
                                select d).First();
                var donations = (from p in db.Donations
                                 where p.EventID == EventID
                                 select p);
                foreach (var don in donations)
                {
                    db.Donations.Remove(don);
                }
                db.FundRaisingEvents.Remove(event1);
                db.SaveChanges();
                MessageBox.Show("You successfully deleted this Grant but the Proposal for this grant has been set to Pending.");
                this.Close();
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
    }
}
