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
    /// Interaction logic for UpdatePatient.xaml
    /// </summary>
    public partial class UpdatePatient : Window
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int patientOQ { get; set; }
        public string notes { get; set; }
        public string gender { get; set; }
        public UpdatePatient(PatientGrid p)
        {
            firstName = p.FirstName;
            lastName = p.LastName;
            patientOQ = p.PatientOQ;
            gender = p.Gender;
            InitializeComponent();
        }

        private void Update_Patient(object sender, RoutedEventArgs e)
        {

        }
    }
}
