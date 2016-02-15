namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class In_Kind_Item
    {
        //Constructor made by Ken
        public In_Kind_Item(int iID, int dID, string name, string description)
        {
            ItemID = iID;
            DonationID = dID;
            ItemName = name;
            ItemDescription = description;
        }

        [Key]
        public int ItemID { get; set; }

        public int DonationID { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

        [Required]
        [StringLength(5000)]
        public string ItemDescription { get; set; }

        public virtual Donation Donation { get; set; }
    }
}
