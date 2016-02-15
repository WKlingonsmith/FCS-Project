namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonorContact")]
    public partial class DonorContact
    {
        //Ken made this constructor
        DonorContact(int cID, int dID, string cFirstName, string cLastName, string cPhone, string cEmail)
        {
            ContactID = cID;
            DonorID = dID;
            ContactFirstName = cFirstName;
            ContactLastName = cLastName;
            ContactPhone = cPhone;
            ContactEmail = cEmail;
        }//end of constructor

        [Key]
        public int ContactID { get; set; }

        public int DonorID { get; set; }

        [Required]
        [StringLength(30)]
        public string ContactFirstName { get; set; }

        [StringLength(30)]
        public string ContactLastName { get; set; }

        [StringLength(10)]
        public string ContactPhone { get; set; }

        [StringLength(700)]
        public string ContactEmail { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
