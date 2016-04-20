using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateNewEvent.xaml
    /// </summary>
    public partial class CreateNewEvent : Window
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }

        public string BeginHour { get; set; }
        public string BeginMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }

        public CreateNewEvent()
        {
            InitializeComponent();
        }

        private void Add_Event(object sender, RoutedEventArgs e)
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
            try
            {
                DateTime help = Convert.ToDateTime(DateRecieved.ToString());
                DateTime startDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(BeginHour), Convert.ToInt32(BeginMinute), 0);
                DateTime endDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(EndHour), Convert.ToInt32(EndMinute), 0);
                decimal timeDiff = (decimal)(endDateTime - startDateTime).TotalHours;

                if (EventName != null && EventName != "" && timeDiff > 0)
                {
                    Models.FCS_DBModel db = new Models.FCS_DBModel();
                    //MessageBox.Show(EventName + "\n" + EventDescription + "\n" + startDateTime + "\n" + endDateTime + "\n" + timeDiff );

                    Models.FundRaisingEvent event1 = new Models.FundRaisingEvent();

                    event1.EventStartDateTime = startDateTime;
                    event1.EventEndDateTime = endDateTime;
                    event1.EventName = EventName;
                    event1.EventDescription = EventDescription;

                    db.FundRaisingEvents.Add(event1);
                    db.SaveChanges();
                    MessageBox.Show("Successfully added Event");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Make sure you input correct data.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Make sure you input correct data.");
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
                catch 
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
