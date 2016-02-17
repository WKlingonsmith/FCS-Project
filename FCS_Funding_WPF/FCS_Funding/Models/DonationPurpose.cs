namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonationPurpose")]
    public partial class DonationPurpose
    {
        /*Ken created this constructor*/
        public DonationPurpose(int dpID, int dID, int pID, decimal dPurposeAmount)
        {
            DonationPurposeID = dpID;
            DonationID = dID;
            PurposeID = pID;
            DonationPurposeAmount = dPurposeAmount;
        }//end of constructor

        public int DonationPurposeID { get; set; }

        public int DonationID { get; set; }

        public int PurposeID { get; set; }

        [Column(TypeName = "money")]
        public decimal DonationPurposeAmount { get; set; }

        public virtual Donation Donation { get; set; }

        public virtual Purpose Purpose { get; set; }
    }
}