using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_DataTesting
{
    public class SessionsDataGrid
    {
        public String ClientName { get; set; }
        public int Sessions { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
        public SessionsDataGrid(String clientname, int sessions, decimal amount, DateTime date, decimal balance)
        {
            ClientName = clientname;
            Sessions = sessions;
            Amount = amount;
            Date = date;
            Balance = balance;
        }
        public SessionsDataGrid()
        { }
    }
}
