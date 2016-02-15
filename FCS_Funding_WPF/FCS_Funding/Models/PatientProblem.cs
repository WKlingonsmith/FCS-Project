namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PatientProblem")]
    public partial class PatientProblem
    {

        //Constructor made by Ken
        public PatientProblem(int probID, int patientID, string probType)
        {
            ProblemID = probID;
            PatientID = patientID;
            ProbelmType = probType;
        }

        [Key]
        public int ProblemID { get; set; }

        public int PatientID { get; set; }

        [Required]
        [StringLength(50)]
        public string ProbelmType { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
