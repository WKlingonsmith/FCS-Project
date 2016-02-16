using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class InKindDonation
    {
        public string ItemName { get; set; }
        public string DonorName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal VolunteerHours { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public InKindDonation(string iName, string dName, string oName, DateTime t, string desc, decimal vh = 0, decimal rph = 0)
        {
            ItemName = iName;
            DonorName = dName;
            OrganizationName = oName;
            EndDateTime = t;
            VolunteerHours = vh;
            RatePerHour = rph;
            Value = vh * rph;
            Description = desc;
        }
    }
}
//<DataGridTextColumn Header="Item" Width="100" Binding="{Binding Path=ItemName}"/>
//<DataGridTextColumn Header="Volunteer" Width="110" Binding="{Binding Path=DonorName}"/>
//<DataGridTextColumn Header="Organization" Width="150" Binding="{Binding Path=OrganizationName}"/>
//<DataGridTextColumn Header="Date Recieved" Width="150" Binding="{Binding Path=EndDateTime}"/>
//<DataGridTextColumn Header="Hours" Width="100" Binding="{Binding Path=VolunteerHours}"/>
//<DataGridTextColumn Header="Rate/Hour" Width="120" Binding="{Binding Path=RatePerHour}"/>
//<DataGridTextColumn Header="Value (hours * rate)" Width="180" Binding="{Binding Path=Value}"/>
//<DataGridTextColumn Header="Description" Width="150" Binding="{Binding Path=Description}"/>