﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FCS_Funding.Models;
using System.Windows.Input;
using FCS_DataTesting;

namespace FCS_Funding.Views.TabViews
{
	using Definition;
	using System;   /// <summary>
					/// Interaction logic for Tab_Sessions.xaml
					/// </summary>
	public partial class Tab_Sessions : UserControl
	{

		public Tab_Sessions()
		{
			InitializeComponent();
		}


		private void Edit_Expense(object sender, MouseButtonEventArgs e)
		{
			try 
			{
				DataGrid dg = sender as DataGrid;

				SessionsGrid p = (SessionsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateSession up = new UpdateSession(p);
				up.ShowDialog();
			}
			catch (Exception error)
			{
			}

			//	Refresh the grid
			Refresh_SessionGrid();
		}

		private void Open_CreateNewSession(object sender, RoutedEventArgs e)
		{
			AppointmentType at = new AppointmentType();
			at.AMPM_Start.SelectedIndex = 0;
			at.AMPM_End.SelectedIndex = 0;
			at.ApptType.SelectedIndex = 0;
			at.ShowDialog();

            //	Refresh the grid
            //Refresh_SessionGrid();
            Refresh_SessionGrid(sender, e);

        }

	/// <summary>
	/// Override that calls the function just below for handlers -only-
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
		private void Refresh_SessionGrid(object sender, RoutedEventArgs e)
		{
			Refresh_SessionGrid();
		}

	/// <summary>
	/// This function refreshes the grid for the Sessions tab with data from the database
	/// </summary>
		private void Refresh_SessionGrid()
		{
			var db = new FCS_DBModel();
			var join1 = from s in db.Staff
						join a in db.Appointments on s.StaffID equals a.StaffID
						join ex in db.Expenses on a.AppointmentID equals ex.AppointmentID
						join et in db.ExpenseTypes on ex.ExpenseTypeID equals et.ExpenseTypeID
						join p in db.Patients on ex.PatientID equals p.PatientID
						select new SessionsGrid
						{
							StaffFirstName = s.StaffFirstName,
							StaffLastName = s.StaffLastName,
							ClientFirstName = p.PatientFirstName,
							ClientLastName = p.PatientLastName,
							AppointmentStart = a.AppointmentStartDate,
							AppointmentEnd = a.AppointmentEndDate,
							ExpenseDueDate = ex.ExpenseDueDate,
							ExpensePaidDate = ex.ExpensePaidDate,
							DonorBill = ex.DonorBill,
							PatientBill = ex.PatientBill,
							TotalExpense = ex.TotalExpenseAmount,
							ExpenseType = et.ExpenseType1,
							ExpenseDescription = et.ExpenseDescription,
							ExpenseID = ex.ExpenseID,
                            CancellationType = a.AppointmentCancelationType
						};

		//	Set the data to the grid
			Session_DataGrid.ItemsSource = join1.ToList();
		}
	}
}
