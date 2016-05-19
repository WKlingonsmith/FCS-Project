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
using FCS_DataTesting;
using FCS_Funding.Models;

namespace FCS_Funding.Views.TabViews
{
	using Definition;

	/// <summary>
	/// Interaction logic for Tab_Events.xaml
	/// </summary>
	public partial class Tab_Events : UserControl
	{
		public string StaffRole { get; set; }

		public Tab_Events()
		{
			InitializeComponent();

			//	Check for permissions
			StaffRole = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().StaffDBRole;

			if (StaffRole == Definition.Basic || StaffRole == Definition.User)
			{
				CreateEven.IsEnabled = false;
			}
			else if (StaffRole == Definition.Admin)
			{
				CreateEven.IsEnabled = true;
			}
		}

		private void Edit_Events(object sender, MouseButtonEventArgs e)
		{
			int Count = Application.Current.Windows.Count;
			if (Count <= 1)
			{
				DataGrid dg = sender as DataGrid;
				EventsDataGrid p = (EventsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateEvent up = new UpdateEvent(p, StaffRole);
				if (StaffRole != Definition.Admin)
				{
					up.Delete.IsEnabled = false;
				}
				if (StaffRole == Definition.Basic)
				{
					up.Delete.IsEnabled = false;
					up.UpEvent.IsEnabled = false;
					up.AddDonation.IsEnabled = false;
					up.AddItem.IsEnabled = false;
					up.AddService.IsEnabled = false;
				}
				if (p.EventStartDateTime.Hour >= 12)
				{
					up.AMPM_Start.SelectedIndex = 1;
				}
				else
				{
					up.AMPM_Start.SelectedIndex = 0;
				}
				if (p.EventEndDateTime.Hour >= 12)
				{
					up.AMPM_End.SelectedIndex = 1;
				}
				else
				{
					up.AMPM_End.SelectedIndex = 0;
				}
				up.DateRecieved.SelectedDate = p.EventStartDateTime;
				up.ShowDialog();
			}
		}


		private void Events_Grid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = (from p in db.FundRaisingEvents
						 select new EventsDataGrid
						 {
							 EventID = p.EventID,
							 EventStartDateTime = p.EventStartDateTime,
							 EventEndDateTime = p.EventEndDateTime,
							 EventName = p.EventName,
							 EventDescription = p.EventDescription
						 });
			var grid = sender as DataGrid;
			grid.ItemsSource = join1.ToList();

		}

		private void CreateNewEvent(object sender, RoutedEventArgs e)
		{
			if (Application.Current.Windows.Count <= 1)
			{
				CreateNewEvent ne = new CreateNewEvent();
				ne.Show();
				ne.AMPM_End.SelectedIndex = 0;
				ne.AMPM_Start.SelectedIndex = 0;
				ne.Topmost = true;
			}
		}

		private void Refresh_Events(object sender, RoutedEventArgs e)
		{
			sender = Event_DataGrid;
			Events_Grid(sender, e);
		}

	}
}
