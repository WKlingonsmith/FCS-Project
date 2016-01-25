using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCS_Funding
{
    class Client
    {
        public string clientName;
        public int clientOQ;
        public string notes;

        public Client(string cName, int cOQ, string note)
        {
            clientName = cName;
            clientOQ = cOQ;
            notes = note;
        }
    }
}
