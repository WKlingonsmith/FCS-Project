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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FCS_Funding.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Report_ClientYTD.xaml
    /// </summary>
    public partial class Report_ClientYTD : UserControl
    {
        public Report_ClientYTD()
        {
            InitializeComponent();
        }
        private void printClientYTDReport_button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                clientYTD.Measure(pageSize);
                clientYTD.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(clientYTD, "Client YTD Report");
            }
        }


        private void generateClientYTDReport_button_Click(object sender, RoutedEventArgs e)
        {
            clientYTD.Visibility = Visibility.Visible;
            printClientYTDReport_button.Visibility = Visibility.Visible;

            generateClientYTDReport_button.Visibility = Visibility.Hidden;
            clientYTDStartDate.Visibility = Visibility.Hidden;
            clientYTDEndDate.Visibility = Visibility.Hidden;
            clientYTDStartDateLabel.Visibility = Visibility.Hidden;
            clientYTDEndDateLabel.Visibility = Visibility.Hidden;
            resetClientYTDReport_button.Visibility = Visibility.Visible;
        }
        private void resetClientYTDReport_button_Click(object sender, RoutedEventArgs e)
        {
            clientYTD.Visibility = Visibility.Hidden;
            printClientYTDReport_button.Visibility = Visibility.Hidden;

            generateClientYTDReport_button.Visibility = Visibility.Visible;
            clientYTDStartDate.Visibility = Visibility.Visible;
            clientYTDEndDate.Visibility = Visibility.Visible;
            clientYTDStartDateLabel.Visibility = Visibility.Visible;
            clientYTDEndDateLabel.Visibility = Visibility.Visible;
            resetClientYTDReport_button.Visibility = Visibility.Hidden;

        }
    }
}
