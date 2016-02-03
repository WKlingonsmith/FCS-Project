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
    /// Interaction logic for CreateNewPatient.xaml
    /// </summary>
    public partial class CreateNewPatient : Window
    {
        public string patientName { get; set; }
        public string patientOQ { get; set; }
        public string notes { get; set; }
        public CreateNewPatient()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.patientName != null && this.patientOQ != null && this.notes != null)
            {
                //add the client to the database
                this.Close();
                MessageBox.Show(this.patientName + " " + this.patientOQ + " " + this.notes);
                MessageBox.Show("Successfully added client.");
                
            }
            else
            {
                MessageBox.Show("Unable to add client");
            }
        }
    }
}
