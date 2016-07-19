using FCS_Funding.Models;
using System;
using System.Linq;
using System.Windows;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateNewDonor.xaml
    /// </summary>
    public partial class CreateNewDonor : Window
    {
        public string DonorAddress1 { get; set; } //
        public string DonorAddress2 { get; set; } //
        public string DonorCity { get; set; } //
        public string DonorState { get; set; } //
        public string DonorType { get; set; } //Not
        public string DonorZip { get; set; } //
        public string OrganizationName { get; set; } //
        public CreateNewDonor()
        {
            InitializeComponent();
			textbox_orgName.Focus();
        }

        private void CreateDonor(object sender, RoutedEventArgs e)
        {
            Determine_DonorType(this.dType.SelectedIndex);
            //DonorFirstName != null && DonorFirstName != "" && DonorLastName != null && DonorLastName != ""
            if (DonorType != null && DonorType != "")
            {
                FCS_DBModel db = new FCS_DBModel();
                if (DonorType == "Organization" || DonorType == "Government" || DonorType == "Insurance")
                {
                    var OrgName = from d in db.Donors
                                  where d.OrganizationName == OrganizationName
                                  select d;
                    if (OrganizationName != null && OrganizationName != "")
                    {
                        //MessageBox.Show(DonorAddress1 + "\n" + DonorAddress2 + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip
                        //    + "\n" + DonorType + "\n" + OrganizationName);
                        Donor d = new Donor();


                        try { d.DonorState = DonorState; } catch { }
                        try { d.DonorCity = DonorCity; } catch { }
                        try { d.DonorZip = DonorZip; } catch { }
                        try { d.DonorAddress2 = DonorAddress2; } catch { }
                        try { d.DonorAddress1 = DonorAddress1; } catch { }
                        d.DonorType = DonorType;
                        d.OrganizationName = OrganizationName;
                        db.Donors.Add(d);

                        db.SaveChanges();
                        
                        this.Close();
                    }
                    else if (OrganizationName == null || OrganizationName == "")
                    {
                        MessageBox.Show("Make sure to add an Organization Name.");
                    }
                    else
                    {
                        MessageBox.Show("There is already an organization with the name selected. \nNote: Number the organizations if any organizations have the same name.");
                    }
                }
                else if(DonorType == "Individual")
                {
                    //MessageBox.Show(DonorAddress1 + "\n" + DonorAddress2 + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip
                    //    + "\n" + DonorType + "\n" + OrganizationName);
                    if(DonorZip == null)
                    {
                        DonorZip = "";
                    }
                    if(DonorState == null)
                    {
                        DonorState = "";
                    }
                    if(DonorZip.Length <= 5 && DonorState.Length <= 2)
                    { 
                        Donor d = new Donor();

                        d.DonorType = DonorType;
                        d.OrganizationName = OrganizationName;
                        d.DonorAddress1 = DonorAddress1;
                        d.DonorAddress2 = DonorAddress2;
                        d.DonorState = DonorState;
                        d.DonorCity = DonorCity;
                        d.DonorZip = DonorZip;

                        CreateIndividualContact cic = new CreateIndividualContact(d);
                        this.Close();
                        cic.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Make sure your state is two digits and your zip is 5 digits.");
                    }
                }
                //its anonymous
                else if(DonorType == "Anonymous")
                {
                    //"Anonymous"
                    int anony = db.Donors.Where(x => x.DonorType == "Anonymous").Select(x => x.DonorType).Count(); //Distinct().First();
                    string Anon = "Anonymous";
                    if (anony < 1)
                    {
                        Donor d = new Donor();
                        d.DonorType = Anon;
                        d.OrganizationName = Anon;
                        d.DonorAddress1 = Anon;
                        d.DonorAddress2 = Anon;
                        d.DonorState = "";
                        d.DonorCity = Anon;
                        d.DonorZip = "";
                        DonorContact dc = new DonorContact();
                        dc.ContactFirstName = Anon;
                        dc.ContactLastName = Anon;
                        dc.ContactPhone = Anon;
                        dc.ContactEmail = Anon;
                        dc.DonorID = d.DonorID;

                        db.Donors.Add(d);
                        db.DonorContacts.Add(dc);
                        db.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Anonymous donor already exists");
                    }
                }
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure to select Donor Type.");
            }

        }

        private void Determine_DonorType(int selection)
        {
            switch (selection)
            {
                case 0:
                    DonorType = "Organization"; break;
                case 1:
                    DonorType = "Individual"; break;
                case 2:
                    DonorType = "Anonymous"; break;
                case 3:
                    DonorType = "Government"; break;
                case 4:
                    DonorType = "Insurance"; break;
            }
		}

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
