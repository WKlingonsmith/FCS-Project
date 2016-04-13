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
    /// Interaction logic for AppointmentType.xaml
    /// </summary>
    public partial class AppointmentType : Window
    {
        public AppointmentType()
        {
            InitializeComponent();
        }

        private void Appt_DropDown(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>()
            { 
                "Group/Individual",    
                "Family"
            };
        }

        private void Select_AppointmentType(object sender, RoutedEventArgs e)
        {
            //indiv/group
            if(ApptType.SelectedIndex == 0)
            {

            }
            //family
            else if(ApptType.SelectedIndex == 1)
            {

            }
        }

        private void ApptType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //indiv/group
            if (ApptType.SelectedIndex == 0)
            {

            }
            //family
            else if (ApptType.SelectedIndex == 1)
            {

            }
        }
    }
}
