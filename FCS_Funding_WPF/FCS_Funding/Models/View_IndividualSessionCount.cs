namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_IndividualSessionCount
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StaffID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string StaffFirstName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string StaffLastName { get; set; }

        [Column("Individual Sessions")]
        public int? Individual_Sessions { get; set; }
    }
}
