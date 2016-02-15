namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Donor")]
    public partial class Donor
    {
        //Ken made this constructor
        public Donor(int dID, string dFirstName, string dLastName, string dType, string oName, string dAddress1,
            string dAddress2, string dState, string dCity, string dZip)
        {
            DonorID = dID;
            DonorFirstName = dFirstName;
            DonorLastName = dLastName;
            DonorType = dType;
            OrganizationName = oName;
            DonorAddress1 = dAddress1;
            DonorAddress2 = dAddress2;
            DonorState = dState;
            DonorCity = dCity;
            DonorZip = dZip;

            Donation = new HashSet<Donation>();
            DonorContact = new HashSet<DonorContact>();
            GrantProposal = new HashSet<GrantProposal>();
            Reminder = new HashSet<Reminder>();
        }

        public int DonorID { get; set; }

        [StringLength(50)]
        public string DonorFirstName { get; set; }

        [StringLength(50)]
        public string DonorLastName { get; set; }

        [Required]
        [StringLength(50)]
        public string DonorType { get; set; }

        [StringLength(250)]
        public string OrganizationName { get; set; }

        [StringLength(50)]
        public string DonorAddress1 { get; set; }

        [StringLength(50)]
        public string DonorAddress2 { get; set; }

        [StringLength(2)]
        public string DonorState { get; set; }

        [StringLength(200)]
        public string DonorCity { get; set; }

        [StringLength(10)]
        public string DonorZip { get; set; }

        public virtual ICollection<Donation> Donation { get; set; }

        public virtual ICollection<DonorContact> DonorContact { get; set; }

        public virtual ICollection<GrantProposal> GrantProposal { get; set; }

        public virtual ICollection<Reminder> Reminder { get; set; }
    }
}
