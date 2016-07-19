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
using FCS_Funding.Models;
namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AddPurpose.xaml
    /// </summary>
    public partial class AddPurpose : Window
    {
        public AddPurpose()
        {
            InitializeComponent();

			PurposeName.Focus();
        }

        private void AddPurposeButton_Click(object sender, RoutedEventArgs e)
        {
            FCS_DBModel db = new FCS_DBModel();
            Purpose purpose = db.Purposes.Create();
            purpose.PurposeName = PurposeName.Text;
            purpose.PurposeDescription = PurposeDescription.Text;
            db.Purposes.Add(purpose);
            try {
                db.SaveChanges();
                
                this.Close();
            }
            catch
            {
                MessageBox.Show("Please make sure all fields are correct");
            }
            
            
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
