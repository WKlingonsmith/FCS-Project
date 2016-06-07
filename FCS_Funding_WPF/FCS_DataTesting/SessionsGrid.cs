using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class SessionsGrid
    {
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public DateTime ExpenseDueDate { get; set; }
        public decimal DonorBill { get; set; }
        public decimal PatientBill { get; set; }
        public decimal TotalExpense { get; set; }
        public DateTime? ExpensePaidDate { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseDescription { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }

        public int ExpenseID { get; set; }
        public string CancellationType { get; set; }

        public SessionsGrid()
        { }
    }
}
