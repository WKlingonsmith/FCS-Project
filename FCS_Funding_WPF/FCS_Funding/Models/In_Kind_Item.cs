using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class In_Kind_Item
    {
        public int ItemID { get; set; }
        public int DonationID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public virtual Donation Donation { get; set; }
    }
}
