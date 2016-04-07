using FCS_Funding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for CreateIndividualContact.xaml
    /// </summary>
    public partial class CreateIndividualContact : Window
    {
        //Contact Information
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        //Donor Information
        public string DonorAddress1 { get; set; } //
        public string DonorAddress2 { get; set; } //
        public string DonorCity { get; set; } //
        public string DonorState { get; set; } //
        public string DonorType { get; set; } //Not
        public string DonorZip { get; set; } //
        public string OrganizationName { get; set; } //

        public CreateIndividualContact(Donor d)
        {
            DonorAddress1 = d.DonorAddress1;
            DonorAddress2 = d.DonorAddress2;
            DonorCity = d.DonorCity;
            DonorState = d.DonorState;
            DonorType = d.DonorType;
            DonorZip = d.DonorZip;
            OrganizationName = d.OrganizationName;
            InitializeComponent();
        }

        private void Add_Contact(object sender, RoutedEventArgs e)
        {
            if (DonorFirstName != null && DonorFirstName != "" && DonorLastName != null && DonorLastName != "" && ContactPhone != null && ContactPhone != ""
                && ContactEmail != null && ContactEmail != "")
            {
                FCS_FundingDBModel db = new FCS_FundingDBModel();
                MessageBox.Show(DonorFirstName + "\n" + DonorLastName + "\n" + ContactPhone + "\n" + ContactEmail + "\n" + DonorAddress1 + "\n" + DonorAddress2
                    + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip + "\n" + DonorType + "\n" + OrganizationName);
                Donor d = new Donor();
                DonorContact dc = new DonorContact();

                d.DonorType = DonorType;
                d.OrganizationName = OrganizationName;
                d.DonorAddress1 = DonorAddress1;
                d.DonorAddress2 = DonorAddress2;
                d.DonorState = DonorState;
                d.DonorCity = DonorCity;
                d.DonorZip = DonorZip;
                db.Donors.Add(d);
                db.SaveChanges();

                dc.ContactFirstName = DonorFirstName;
                dc.ContactLastName = DonorLastName;
                dc.ContactPhone = ContactPhone;
                dc.ContactEmail = ContactEmail;
                dc.DonorID = d.DonorID;
                db.DonorContacts.Add(dc);
                db.SaveChanges();

                MessageBox.Show("Successfully added Donor!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Add the correct fields.");
            }
        }
    }
}