using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Patient
    {
        public Patient(int pOQ, int houseID, string fName, string lName, string gen, string age, string ethnic, DateTime time, bool head, string relation)
        {
            PatientOQ = pOQ;
            HouseholdID = houseID;
            PatientFirstName = fName;
            PatientLastName = lName;
            PatientGender = gen;
            PatientAgeGroup = age;
            PatientEthnicity = ethnic;
            NewClientIntakeHour = time;
            IsHead = head;
            RelationToHead = relation;
            this.Expenses = new List<Expense>();
            this.PatientProblems = new List<PatientProblem>();
        }
        public Patient()
        {
            this.Expenses = new List<Expense>();
            this.PatientProblems = new List<PatientProblem>();
        }

        public int PatientID { get; set; }
        public int PatientOQ { get; set; }
        public int HouseholdID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientGender { get; set; }
        public string PatientAgeGroup { get; set; }
        public string PatientEthnicity { get; set; }
        public System.DateTime NewClientIntakeHour { get; set; }
        public bool IsHead { get; set; }
        public string RelationToHead { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual PatientHousehold PatientHousehold { get; set; }
        public virtual ICollection<PatientProblem> PatientProblems { get; set; }
    }
}
