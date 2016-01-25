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

namespace FCS_Funding
{
    /// <summary>
    /// Interaction logic for CreateNewClient.xaml
    /// </summary>
    public partial class CreateNewClient : Window
    {
        public string clientName { get; set; }
        public string clientOQ { get; set; }
        public string notes { get; set; }

        public CreateNewClient()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.clientName != null && this.clientOQ != null && this.notes != null)
            {
                //add the client to the database
                this.Close();
                MessageBox.Show(this.clientName + " " + this.clientOQ + " " + this.notes);
                MessageBox.Show("Successfully added client.");
            }
            else
            {
                MessageBox.Show("Unable to add client");
            }
        }
    }
}
