using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class DonorContact
    {
        public int ContactID { get; set; }
        public int DonorID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public virtual Donor Donor { get; set; }
    }
}
