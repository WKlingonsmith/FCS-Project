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
    /// Interaction logic for CreateHousehold.xaml
    /// </summary>
    public partial class CreateHousehold : Window
    {
        //private static enum HeadOfHousehold { Mother, Father };
        private string HeadOfHousehold { get; set; }
        private string Income { get; set; }
        public CreateHousehold()
        {
            InitializeComponent();
        }

        private void Add_Household(object sender, RoutedEventArgs e)
        {
            if (this.headOfHousehold.SelectedIndex == 0)
            {
                HeadOfHousehold = "Mother";
            }
            else if (this.headOfHousehold.SelectedIndex == 1)
            {
                HeadOfHousehold = "Father";
            }

            if(this.income.SelectedIndex == 0)
            {
                Income = "$0-9,999";
            }
            else if (this.income.SelectedIndex == 1)
            {
                Income = "$10,000-14,999";
            }
            else if (this.income.SelectedIndex == 2)
            {
                Income = "$15,000-24,000";
            }
            else if (this.income.SelectedIndex == 3)
            {
                Income = "$25,000-34,999";
            }
            else if (this.income.SelectedIndex == 4)
            {
                Income = "$35,000+";
            }
            if (HeadOfHousehold == null || Income == null)
            {
                MessageBox.Show("Make sure you select an income and head of household");
            }
            else
            {
                MessageBox.Show(HeadOfHousehold + "\n" + Income);
            }
        }

    }
}
