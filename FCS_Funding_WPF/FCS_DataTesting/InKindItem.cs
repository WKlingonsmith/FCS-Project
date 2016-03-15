using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class InKindItem
    {
        public string ItemName { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime DonationDate { get; set; }
        public string ItemDescription { get; set; }
        public int DonorID { get; set; }
        public DateTime DateRecieved { get; set; }
        public string Description { get; set; }
        public InKindItem(string iName, string dFName, string dLName, string oName, DateTime t, string desc)
        {
            ItemName = iName;
            DonorFirstName = dFName;
            DonorLastName = dLName;
            OrganizationName = oName;
            DonationDate = t;
            ItemDescription = desc;
        }
        public InKindItem()
        {
        }
    }
}