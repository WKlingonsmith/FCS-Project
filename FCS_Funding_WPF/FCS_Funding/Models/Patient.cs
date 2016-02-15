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
        /*Ken set up this constructor*/
        public Patient(int pID, int pOQ, int houseID, string fName, string lName, string pGender, 
            string pAgeGroup, string pEthnicity, DateTime intakeHour, bool head)
        {
            PatientID = pID; 
            PatientOQ = pOQ;
            HouseholdID = houseID;
            PatientFirstName = fName;
            PatientLastName = lName;
            PatientGender = pGender;
            PatientAgeGroup = pAgeGroup;
            PatientEthnicity = pEthnicity;
            NewClientIntakeHour = intakeHour;
            IsHead = head;
            Expense = new HashSet<Expense>();
            PatientProblem = new HashSet<PatientProblem>();
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

        public virtual ICollection<Expense> Expense { get; set; }

        public virtual PatientHousehold PatientHousehold { get; set; }

        public virtual ICollection<PatientProblem> PatientProblem { get; set; }
    }
}
