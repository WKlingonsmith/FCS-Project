using FCS_DataTesting;
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
    /// Interaction logic for UpdateIndividualDonor.xaml
    /// </summary>
    public partial class UpdateIndividualDonor : Window
    {
        public string DonorFirstName    { get; set; }
        public string DonorLastName     { get; set; }
        public string OrganizationName  { get; set; }
        public string DonorAddress1     { get; set; }
        public string DonorAddress2     { get; set; }
        public string ContactPhone      { get; set; }
        public string ContactEmail      { get; set; }
        public string DonorState        { get; set; }
        public string DonorCity         { get; set; }
        public string DonorZip          { get; set; }
        public int    DonorID           { get; set; }
        public int ContactID { get; set; }
        public UpdateIndividualDonor(DonorsDataGrid p, Models.DonorContact d)
        {
            DonorFirstName = p.DonorFirstName;
            DonorLastName = p.DonorLastName;
            OrganizationName = p.OrganizationName;
            DonorAddress1 = p.DonorAddress1;
            DonorAddress2 = p.DonorAddress2;
            ContactPhone = d.ContactPhone;
            ContactEmail = d.ContactEmail;
            DonorState = p.DonorState;
            DonorCity = p.DonorCity;
            DonorZip = p.DonorZip;
            DonorID = p.DonorID;
            ContactID = d.ContactID;

            InitializeComponent();
        }

        private void CreateDonor(object sender, RoutedEventArgs e)
        {

        }
    }
}