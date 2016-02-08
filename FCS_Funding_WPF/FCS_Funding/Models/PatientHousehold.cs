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
        public PatientHousehold(int housePop, string houseIncome)
        {
            HouseholdPopulation = housePop;
            HouseholdIncomeBracket = houseIncome;
            Patient = new HashSet<Patient>();
        }

        [Key]
        public int HouseholdID { get; set;  }

        public int HouseholdPopulation { get; set; }

        [Required]
        [StringLength(15)]
        public string HouseholdIncomeBracket { get; set; }

        [Required]
        [StringLength(50)]
        public string HouseholdCounty { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }

        public void addHousehold(PatientHousehold ph)
        {
            //this.FCS_DB = from PatientHousehold in FCS_DB ActivationContext
        }
    }
}
