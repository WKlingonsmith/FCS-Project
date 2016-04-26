using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCS_Funding.Models;
namespace FCS_Funding.Views
{
    class DeletePurposes
    {
        public void deletePurpose(int DonationID)
        {
            FCS_DBModel db = new FCS_DBModel();
            var donPurp = db.DonationPurposes.Where(x => x.DonationID == DonationID);
            if (donPurp != null)
            {
                foreach (var item in donPurp)
                {
                    db.DonationPurposes.Remove(item);
                }
                db.SaveChanges();
            }
        }
    }
}
