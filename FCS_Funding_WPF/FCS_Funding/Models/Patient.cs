namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Patient")]
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            Expenses = new HashSet<Expense>();
            PatientProblems = new HashSet<PatientProblem>();
        }

        public int PatientID { get; set; }

        public int PatientOQ { get; set; }

        public int HouseholdID { get; set; }

        [Required]
        [StringLength(50)]
        public string PatientFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string PatientLastName { get; set; }

        [StringLength(6)]
        public string PatientGender { get; set; }

        [StringLength(6)]
        public string PatientAgeGroup { get; set; }

        [StringLength(50)]
        public string PatientEthnicity { get; set; }

        public DateTime NewClientIntakeHour { get; set; }

        public bool IsHead { get; set; }

        [Required]
        [StringLength(30)]
        public string RelationToHead { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual PatientHousehold PatientHousehold { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientProblem> PatientProblems { get; set; }
    }
}
