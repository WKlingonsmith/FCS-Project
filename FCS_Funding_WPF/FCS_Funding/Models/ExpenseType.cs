using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class ExpenseType
    {
        public ExpenseType()
        {
            this.Expenses = new List<Expense>();
        }

        public int ExpenseTypeID { get; set; }
        public string ExpenseType1 { get; set; }
        public string ExpenseDescription { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
