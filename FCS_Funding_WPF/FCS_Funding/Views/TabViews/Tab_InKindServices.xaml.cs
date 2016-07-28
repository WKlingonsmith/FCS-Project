using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FCS_DataTesting;
using FCS_Funding.Models;

namespace FCS_Funding.Views.TabViews
{
	using Definition;

	/// <summary>
	/// Interaction logic for Tab_InKindServices.xaml
	/// </summary>
	public partial class Tab_InKindServices : UserControl
	{
		public Tab_InKindServices()
		{
			InitializeComponent();
		}

		private void Edit_InKindService(object sender, MouseButtonEventArgs e)
		{
			DataGrid dg = sender as DataGrid;

			InKindService p = (InKindService)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
			UpdateInKindService up = new UpdateInKindService(p);

			up.DateRecieved.SelectedDate = p.StartDateTime;

			if (p.StartDateTime.Hour >= 12)
			{
				up.AMPM_Start.SelectedIndex = 1;
			}
			else
			{
				up.AMPM_Start.SelectedIndex = 0;
			}
			if (p.EndDateTime.Hour >= 12)
			{
				up.AMPM_End.SelectedIndex = 1;
			}
			else
			{
				up.AMPM_End.SelectedIndex = 0;
			}
			up.ShowDialog();
            Refresh_InKindServiceGrid(sender, e);
		}
		private void Refresh_InKindServiceGrid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = (from p in db.Donors
						 join dc in db.DonorContacts on p.DonorID equals dc.DonorID
						 join d in db.Donations on p.DonorID equals d.DonorID
						 join ki in db.In_Kind_Service on d.DonationID equals ki.DonationID
						 where (p.DonorType == "Anonymous" || p.DonorType == "Individual")
						 && d.EventID == null
						 select new InKindService
						 {
							 DonorID = p.DonorID,
							 DonationID = d.DonationID,
							 ServiceID = ki.ServiceID,
							 DonorFirstName = dc.ContactFirstName,
							 DonorLastName = dc.ContactLastName,
							 StartDateTime = ki.StartDateTime,
							 EndDateTime = ki.EndDateTime,
							 RatePerHour = ki.RatePerHour,
							 ServiceDescription = ki.ServiceDescription,
							 Length = ki.ServiceLength,
							 Value = ki.ServiceValue
						 });

			Service_DataGrid.ItemsSource = join1.ToList();
		}

		private void Add_InKind_Service(object sender, RoutedEventArgs e)
		{
			AddInKindService iks = new AddInKindService(false, -1);
			iks.AMPM_End.SelectedIndex = 0;
			iks.AMPM_Start.SelectedIndex = 0;
			iks.ShowDialog();

			Refresh_InKindServiceGrid(sender, e);
		}
	}
}
