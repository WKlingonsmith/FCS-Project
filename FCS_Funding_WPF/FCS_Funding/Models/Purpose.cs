using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Purpose
    {
        public Purpose()
        {
            this.DonationPurposes = new List<DonationPurpose>();
        }

        public int PurposeID { get; set; }
        public string PurposeName { get; set; }
        public string PurposeDescription { get; set; }
        public virtual ICollection<DonationPurpose> DonationPurposes { get; set; }
    }
}
