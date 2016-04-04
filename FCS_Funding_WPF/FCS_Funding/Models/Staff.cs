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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Staff()
        {
            Appointments = new HashSet<Appointment>();
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

        [StringLength(50)]
        public string StaffUserName { get; set; }

        [StringLength(50)]
        public string StaffPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string StaffDBRole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
