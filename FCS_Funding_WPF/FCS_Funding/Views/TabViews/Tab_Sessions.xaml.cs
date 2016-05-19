using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FCS_Funding.Models;
using System.Windows.Input;
using FCS_DataTesting;

namespace FCS_Funding.Views.TabViews
{
	using Definition;
	/// <summary>
	/// Interaction logic for Tab_Sessions.xaml
	/// </summary>
	public partial class Tab_Sessions : UserControl
	{
		public string StaffRole { get; set; }

		public Tab_Sessions()
		{
			InitializeComponent();

			//	Check for permissions
			StaffRole = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().StaffDBRole;

			if (StaffRole == Definition.Basic || StaffRole == Definition.User)
			{
				CreateNewsession.IsEnabled = false;
			}
			else if (StaffRole == Definition.Admin)
			{
				CreateNewsession.IsEnabled = true;
			}
		}


		private void Sessions_Grid(object sender, RoutedEventArgs e)
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
							ExpenseID = ex.ExpenseID
						};
			//... Assign ItemsSource of DataGrid.
			var grid = sender as DataGrid;
			grid.ItemsSource = join1.ToList();
		}

		private void Refresh_Session(object sender, RoutedEventArgs e)
		{
			sender = Session_DataGrid;
			Sessions_Grid(sender, e);
		}

		private void Edit_Expense(object sender, MouseButtonEventArgs e)
		{
			if (StaffRole == Definition.Admin)
			{
				DataGrid dg = sender as DataGrid;

				SessionsGrid p = (SessionsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateSession up = new UpdateSession(p);
				up.ShowDialog();
			}
		}

		private void Open_CreateNewSession(object sender, RoutedEventArgs e)
		{
			AppointmentType at = new AppointmentType();
			at.AMPM_Start.SelectedIndex = 0;
			at.AMPM_End.SelectedIndex = 0;
			at.ApptType.SelectedIndex = 0;
			at.ShowDialog();
		}

	}
}
