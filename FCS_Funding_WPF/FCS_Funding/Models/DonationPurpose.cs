using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class DonationPurpose
    {
        public int DonationPurposeID { get; set; }
        public int DonationID { get; set; }
        public int PurposeID { get; set; }
        public decimal DonationPurposeAmount { get; set; }
        public virtual Donation Donation { get; set; }
        public virtual Purpose Purpose { get; set; }
    }
}
