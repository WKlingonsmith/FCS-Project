namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExpenseType")]
    public partial class ExpenseType
    {
        //Ken made this constructor, FOR SCIENCE
        public ExpenseType(int etID, string et1, string eDescription)
        {
            ExpenseTypeID = etID;
            ExpenseType1 = et1;
            ExpenseDescription = eDescription;

            Expense = new HashSet<Expense>();
        }

        public int ExpenseTypeID { get; set; }

        [Column("ExpenseType")]
        [Required]
        [StringLength(50)]
        public string ExpenseType1 { get; set; }

        [StringLength(5000)]
        public string ExpenseDescription { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
    }
}
