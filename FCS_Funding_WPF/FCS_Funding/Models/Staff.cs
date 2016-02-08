namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Staff")]
    public partial class Staff
    {
        public Staff()
        {
            Appointment = new HashSet<Appointment>();
        }

        public int StaffID { get; set; }

        [Required]
        [StringLength(50)]
        public string StaffFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string StaffLastName { get; set; }

        [Required]
        [StringLength(50)]
        public string StaffTitle { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
    }
}
