using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCS_Funding.Models;
using FCS_Funding.Views;
namespace FCS_Funding_Test
{
    [TestClass]
    public class CreateNewHouseold_Test
    {
        public PatientHousehold ph;

        [TestInitialize]
        public void Initialize()
        {
            //ph = new PatientHousehold(4, "$0-9,999", "Davis");
        }
        [TestMethod]
        public void CreateNewHousehold_Constructor()
        {
            //Assert.AreEqual(4, ph.HouseholdPopulation);
        }

        [TestMethod]
        public void CreateNewHousehold_AddHouseold()
        { 
        
        }
    }
}
