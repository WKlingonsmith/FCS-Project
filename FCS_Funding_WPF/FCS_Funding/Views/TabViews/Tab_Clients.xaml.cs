using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FCS_Funding.Views;
using FCS_DataTesting;
using FCS_Funding.Models;
using System.Collections.Generic;
using FCS_Funding.Views.Windows;

namespace FCS_Funding.Views.TabViews
{
	using Definition;

	/// <summary>
	/// Interaction logic for Tab_Clients.xaml
	/// </summary>
	public partial class Tab_Clients : UserControl
    {

		public string StaffRole{ get; set; }

	//	Initialization
		public Tab_Clients()
        {
            InitializeComponent();
			
			//	Populate the combobox
			combobox_Search.ItemsSource = new List<string> { "", Definition.Filter_ClientOQ, Definition.Filter_FirstName, Definition.Filter_LastName, Definition.Filter_Gender, Definition.Filter_AgeGroup, Definition.Filter_Ethnicity };
		}

//	-----------------------------------------------------------------------------
		/// <summary>
		/// This method is for when the EditPatient button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditPatient(object sender, RoutedEventArgs e)
		{
			try
			{
				DataGrid dg = sender as DataGrid;

				PatientGrid p = (PatientGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				Window_Client clientWindow = new Window_Client(StaffRole, p);
				
				clientWindow.ShowDialog();
			}
			catch (Exception error)
			{
			}

		//	Refresh the grid
			Refresh_ClientGrid(sender, e);
		}

		private void Open_CreateNewPatient(object sender, RoutedEventArgs e)
		{
			Window_Client ch = new Window_Client(StaffRole, null);
			ch.combobox_Gender.SelectedIndex = 0;
			ch.combobox_AgeGroup.SelectedIndex = 0;
			ch.combobox_ethnicity.SelectedIndex = 0;
			ch.ShowDialog();

		//	Refresh the grid after closing the create new patient
			Refresh_ClientGrid(sender, e);
		}

		//	-----------------------------------------------------------------------------

		/// <summary>
		/// This is to handle the refreshing AND filtering of the Client Page
		/// </summary>
		private void Refresh_ClientGrid(object sender, RoutedEventArgs e)
		{
			string filterText = textbox_Search.Text;
			string selectedItem = combobox_Search.Text;

			var db = new FCS_DBModel();
			var clients = from patient in db.Patients
						  join patienthouse in db.PatientHouseholds on patient.HouseholdID equals patienthouse.HouseholdID
						  select new PatientGrid
						  {
							  PatientOQ = patient.PatientOQ,
							  PatientID = patient.PatientID,
							  FirstName = patient.PatientFirstName,
							  LastName = patient.PatientLastName,
							  Gender = patient.PatientGender,
							  AgeGroup = patient.PatientAgeGroup,
							  Ethnicity = patient.PatientEthnicity,
							  Time = patient.NewClientIntakeHour,
							  IsHead = patient.IsHead,
							  RelationToHead = patient.RelationToHead
						  };

			switch (selectedItem)
			{
				case Definition.Filter_AgeGroup:
					clients = clients.Where(x => x.AgeGroup.Contains(filterText));
					break;
				case Definition.Filter_ClientOQ:
					clients = clients.Where(x => x.PatientOQ.Contains(filterText));
					break;
				case Definition.Filter_Ethnicity:
					clients = clients.Where(x => x.Ethnicity.Contains(filterText));
					break;
				case Definition.Filter_FirstName:
					clients = clients.Where(x => x.FirstName.Contains(filterText));
					break;
				case Definition.Filter_LastName:
					clients = clients.Where(x => x.LastName.Contains(filterText));
					break;
				default:
					break;
			}

		//	Set the patient grid to have the (possibly limited) items
			PatientGrid.ItemsSource = clients.ToList();

			GC.Collect();
		}
	}
}
