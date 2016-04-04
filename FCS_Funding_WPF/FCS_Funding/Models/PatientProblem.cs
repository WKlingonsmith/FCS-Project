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
        public int PatientProblemID { get; set; }

        public int PatientID { get; set; }

        public int ProblemID { get; set; }

        [Required]
        [StringLength(50)]
        public string ProbelmType { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Problem Problem { get; set; }
    }
}
