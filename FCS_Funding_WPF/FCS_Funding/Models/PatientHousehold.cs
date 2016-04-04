namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PatientHousehold")]
    public partial class PatientHousehold
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PatientHousehold()
        {
            Patients = new HashSet<Patient>();
        }

        [Key]
        public int HouseholdID { get; set; }

        public int HouseholdPopulation { get; set; }

        [Required]
        [StringLength(15)]
        public string HouseholdIncomeBracket { get; set; }

        [Required]
        [StringLength(50)]
        public string HouseholdCounty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
