using FCS_DataTesting;
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
	/// Interaction logic for UpdateSession.xaml
	/// </summary>
	public partial class UpdateSession : Window
	{
		private string staffFirstName;
		private string staffLastName;
		private string clientFirstName;
		private string clientLastName;
		private DateTime? appointmentStart;
		private DateTime? appointmentEnd;
		private DateTime? expenseDueDate;
		private DateTime? expensePaidDate;
		private string cancellationType;
		private int expenseID;
		private string expenseType;
		private object p;
		private decimal totalExpense;
		private decimal patientBill;
		private decimal donorBill;
		private string expenseDescription;
		private decimal oldDonorBill;
		private decimal newDonorBill;
		private int? oldDonationID;

		public SessionsGrid Session { get; set; }

		public string BeginHour { get; set; }
		public string BeginMinute { get; set; }
		public string EndHour { get; set; }
		public string EndMinute { get; set; }
		public int ExpenseTypeID { get; set; }

		List<PatientGrid> TotalGroup { get; set; }
		List<PatientGrid> Patients { get; set; }
		public bool ShouldRefreshPatients { get; set; }
		public int StaffID { get; set; }
		public DateTime ExpenseDueDate { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		PatientGrid Individual { get; set; }
		public string ClientFirstName { get; set; }
		public string ClientLastName { get; set; }
		public string ClientOQNumber { get; set; }

		public decimal PatientBill { get; set; }
		public decimal DonorBill { get; set; }
		public int AppointmentID { get; set; }


		public UpdateSession(SessionsGrid sg)
		{
			Session = sg;

			// Initialize the variables and such
			staffFirstName = sg.StaffFirstName;
			staffLastName = sg.StaffLastName;
			clientFirstName = sg.ClientFirstName;
			clientLastName = sg.ClientLastName;
			appointmentStart = sg.AppointmentStart;
			appointmentEnd = sg.AppointmentEnd;
			expenseDueDate = sg.ExpenseDueDate;
			expensePaidDate = sg.ExpensePaidDate;
			donorBill = sg.DonorBill;
			patientBill = sg.PatientBill;
			totalExpense = sg.TotalExpense;
			expenseType = sg.ExpenseType;
			expenseDescription = sg.ExpenseDescription;
			expenseID = sg.ExpenseID;
			cancellationType = sg.CancellationType;

			oldDonorBill = donorBill;
			newDonorBill = donorBill;

			// Initialize the UI
			InitializeComponent();

			// Disable the patient's first and last name fields
			FN.IsEnabled = false;
			LN.IsEnabled = false;

			//	Select the appointment type
			ApptType.SelectedItem = sg.ExpenseType;

			//	Fill out the information in the form
			FN.Text = sg.ClientFirstName;
			LN.Text = sg.ClientLastName;
			Copay.Text = Math.Round(sg.PatientBill, 2).ToString();
			Deduction.Text = Math.Round(sg.DonorBill, 2).ToString();


			Models.FCS_DBModel db = new Models.FCS_DBModel();
			oldDonationID = (from d in db.Expenses
							 where d.ExpenseID == expenseID
							 select d.DonationID).Distinct().FirstOrDefault();

			var oldDonation = (from d in db.Donations
							   where d.DonationID == oldDonationID
							   select d).FirstOrDefault();

			//	If there is a grant
			if (oldDonation.GrantProposalID != null)
			{
				MoneyDonation.Visibility = Visibility.Hidden;

				var oldGrantName = (from d in db.GrantProposals
									where d.GrantProposalID == oldDonation.GrantProposalID
									select d.GrantName).Distinct().FirstOrDefault();

				DonorDeduction.IsChecked = true;
				Grant.SelectedItem = oldGrantName;
			}
			//	If it is a money donation
			else if (oldDonation != null)
			{
				//	UI Updates for the money donation
				DonorDeduction.IsChecked = false;
				Grant.Visibility = Visibility.Hidden;

				//	Get the money donation
				var oldDonorType = (from d in db.Donors
									where d.DonorID == oldDonation.DonorID
									select d.DonorType).Distinct().FirstOrDefault();


				if (oldDonorType == "Organization" || oldDonorType == "Government")
				{

					MoneyDonation.SelectedItem = (from d in db.Donations
												  join dn in db.Donors on d.DonorID equals dn.DonorID
												  where d.DonationID == oldDonationID
												  && dn.DonorID == oldDonation.DonorID
												  select d.DonationID.ToString() + ", " + dn.OrganizationName + ", " + d.DonationAmountRemaining.ToString()).Distinct().FirstOrDefault();
				}
				else if (oldDonorType == "Anonymous" || oldDonorType == "Individual")
				{
					MoneyDonation.SelectedItem = (from d in db.Donations
												  join dn in db.Donors on d.DonorID equals dn.DonorID
												  join c in db.DonorContacts on dn.DonorID equals c.DonorID
												  where d.DonationID == oldDonationID
												  && dn.DonorID == oldDonation.DonorID
												  select d.DonationID + ", " + d.DonationAmountRemaining + ", " + c.ContactFirstName + ", " + c.ContactLastName).Distinct().FirstOrDefault();
				}
			}

			Staff.SelectedItem = (from o in db.Staff
								  where o.StaffFirstName == staffFirstName && o.StaffLastName == staffLastName
								  select o.StaffFirstName + " " + o.StaffLastName + ", " + o.StaffUserName).Distinct().FirstOrDefault();

			//	If the patient's due is paid
			if (expensePaidDate != null)
			{
				IsPaid.IsChecked = true;
				ExpensePaidDate.IsEnabled = true;
				ExpensePaidDate.Text = sg.ExpensePaidDate.ToString();
			}

			//	Set the date stuff
			DateRecieved.Text = sg.AppointmentStart.ToString();

			//	Start Hour Stuff
			if (sg.AppointmentStart.Hour >= 12)
			{
				if (sg.AppointmentStart.Hour % 12 != 0)
					StartHour.Text = (sg.AppointmentStart.Hour - 12).ToString();
				else
					StartHour.Text = sg.AppointmentStart.Hour.ToString();

				AMPM_Start.SelectedItem = "PM";
			}
			else
			{
				if (sg.AppointmentStart.Hour == 0)
					StartHour.Text = (sg.AppointmentStart.Hour + 12).ToString();
				else
					StartHour.Text = sg.AppointmentStart.Hour.ToString();

				AMPM_Start.SelectedItem = "AM";
			}

			//	End Hour Stuff
			if (sg.AppointmentEnd.Hour >= 12)
			{
				if (sg.AppointmentEnd.Hour % 12 != 0)
					End_Hour.Text = (sg.AppointmentEnd.Hour - 12).ToString();
				else
					End_Hour.Text = sg.AppointmentEnd.Hour.ToString();

				AMPM_End.SelectedItem = "PM";
			}
			else
			{
				if (sg.AppointmentEnd.Hour == 0)
					End_Hour.Text = (sg.AppointmentEnd.Hour + 12).ToString();
				else
					End_Hour.Text = sg.AppointmentEnd.Hour.ToString();

				AMPM_End.SelectedItem = "AM";
			}

			//	Setting the minutes
			StartMinute.Text = DisplayMinuteConversion(sg.AppointmentStart.Minute.ToString());
			End_Minute.Text = DisplayMinuteConversion(sg.AppointmentEnd.Minute.ToString());

			//	Setting the cancellation type
			CancellationType.SelectedItem = sg.CancellationType;
		}

		private void MoneyDonation_DropDown(object sender, RoutedEventArgs e)
		{
			Models.FCS_DBModel db = new Models.FCS_DBModel();
			var join2 = (from d in db.Donations
						 join dn in db.Donors on d.DonorID equals dn.DonorID
						 where (dn.DonorType == "Organization" || dn.DonorType == "Government" || dn.DonorType == "Insurance")
						 select d.DonationID.ToString() + ", " + dn.OrganizationName + ", " + d.DonationAmountRemaining.ToString()).Union
						 (from d in db.Donations
						  join dn in db.Donors on d.DonorID equals dn.DonorID
						  join c in db.DonorContacts on dn.DonorID equals c.DonorID
						  where (dn.DonorType == "Anonymous" || dn.DonorType == "Individual")
						  && d.GrantProposalID == null
						  select d.DonationID + ", " + d.DonationAmountRemaining + ", " + c.ContactFirstName + ", " + c.ContactLastName).ToList();
			//var query = (from d in db.Donations
			//             where d.GrantProposalID == null
			//             select d.DonationID.ToString() + d.DonationPurposes).ToList();

			var box = sender as ComboBox;
			box.ItemsSource = join2;
		}

		private void Grants_DropDown(object sender, RoutedEventArgs e)
		{
			Models.FCS_DBModel db = new Models.FCS_DBModel();
			var query = (from o in db.GrantProposals
						 where o.GrantStatus == "Accepted"
						 select o.GrantName).ToList();

			var box = sender as ComboBox;
			box.ItemsSource = query;
		}

		private void Appt_DropDown(object sender, RoutedEventArgs e)
		{
			var box = sender as ComboBox;
			box.ItemsSource = new List<string>()
			{
				"Individual",
				"Family",
				"Group"
			};
		}

		private void Staff_DropDown(object sender, RoutedEventArgs e)
		{
			Models.FCS_DBModel db = new Models.FCS_DBModel();
			var query = (from o in db.Staff
						 select o.StaffFirstName + " " + o.StaffLastName + ", " + o.StaffUserName).ToList();

			var box = sender as ComboBox;
			box.ItemsSource = query;
		}

		private void AM_PM_Dropdown(object sender, RoutedEventArgs e)
		{
			var box = sender as ComboBox;
			box.ItemsSource = new List<string>() { "AM", "PM" };
		}

		private void Update_Expense(object sender, RoutedEventArgs e)
		{
			try
			{
				//Database for use throughout Update_Expense
				Models.FCS_DBModel db = new Models.FCS_DBModel();

				//Update Appointment Type
				//individual (1st option) (ExpenseTypeID = 1 in database)
				//group (3rd option) (ExpenseTypeID = 2 in database)
				int newExpenseTypeID = 0;
				switch (ApptType.SelectedIndex)
				{
					case 0:
						newExpenseTypeID = 1;
						break;
					case 1:
						newExpenseTypeID = 3;
						break;
					case 2:
						newExpenseTypeID = 2;
						break;
				}

				//Update Donor/Insurance Deduction
				newDonorBill = Decimal.Parse(Deduction.Text);

				//Update Client Copay in Update All
				//Update Grant/MoneyDonation and Donation
				var oldDonation = (from d in db.Donations
								   where d.DonationID == oldDonationID
								   select d).FirstOrDefault();

				var newDonationQuery = (from d in db.Donations
										select d);

				var expense = (from exp in db.Expenses
							   where exp.ExpenseID == Session.ExpenseID
							   select exp).FirstOrDefault();

				if ((bool)DonorDeduction.IsChecked)
				{
					string newGrant = Grant.SelectedValue.ToString();
					var newGrantproposalID = (from g in db.GrantProposals
											  where g.GrantName == newGrant
											  select g.GrantProposalID).Distinct().FirstOrDefault();

					newDonationQuery = newDonationQuery.Where(x => x.GrantProposalID == newGrantproposalID);
				}
				else
				{
					string[] monDonSeparators = new string[] { ", " };
					string monDon = MoneyDonation.SelectedValue.ToString();
					//MessageBox.Show(ItemName + "\n" + ItemDescription + "\n" + DateRecieved + "\n" + Indiv);
					string[] monDonWords = monDon.Split(monDonSeparators, StringSplitOptions.None);
					int newDonationID = Convert.ToInt32(monDonWords[0]);

					newDonationQuery = newDonationQuery.Where(x => x.DonationID == newDonationID);
				}

				var newDonation = newDonationQuery.FirstOrDefault();

				//	If the new donation is different, check to see if the amount is enough
				decimal amountRemaining = 0;

				if (newDonation.DonationID != oldDonation.DonationID)
				{
					amountRemaining = newDonation.DonationAmountRemaining;
				}
				else
				{
					amountRemaining = newDonation.DonationAmountRemaining + oldDonorBill;
				}

				if (amountRemaining < newDonorBill)
				{
					MessageBox.Show("That donation does not have enough money.");
					return;
				}

				oldDonation.DonationAmountRemaining += oldDonorBill;
				newDonation.DonationAmountRemaining -= newDonorBill;

				expense.DonationID = newDonation.DonationID;

				//Update Therapist
				string[] therSeparators = new string[] { ", ", " " };
				string staff = Staff.SelectedValue.ToString();
				string[] therWords = staff.Split(therSeparators, StringSplitOptions.None);
				string FName = therWords[0];
				string LName = therWords[1];
				string username = therWords[2];
				var newStaffID = (from dc in db.Staff
								  where dc.StaffFirstName == FName && dc.StaffLastName == LName && dc.StaffUserName == username
								  select dc.StaffID).Distinct().FirstOrDefault();

				//Update PaidBill and Date is done when everything is updated

				//Update Appointment Time and Date

				if (AMPM_Start.SelectedValue.ToString() == "PM" && Convert.ToInt32(BeginHour) != 12)
				{
					BeginHour = (Convert.ToInt32(BeginHour) + 12).ToString();
				}
				if (AMPM_End.SelectedValue.ToString() == "PM" && Convert.ToInt32(EndHour) != 12)
				{
					EndHour = (Convert.ToInt32(EndHour) + 12).ToString();
				}
				if (AMPM_Start.SelectedValue.ToString() == "AM" && Convert.ToInt32(BeginHour) == 12)
				{
					BeginHour = (Convert.ToInt32(BeginHour) - 12).ToString();
				}
				if (AMPM_End.SelectedValue.ToString() == "AM" && Convert.ToInt32(EndHour) == 12)
				{
					EndHour = (Convert.ToInt32(EndHour) - 12).ToString();
				}

				DateTime help = Convert.ToDateTime(DateRecieved.ToString());
				DateTime newStartDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(BeginHour), Convert.ToInt32(BeginMinute), 0);
				DateTime newEndDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(EndHour), Convert.ToInt32(EndMinute), 0);
				DateTime newExpenseDueDate = newStartDateTime.AddDays(30);

				//Update Cancellation Type
				string newCancellationType = "Not Cxl";
				switch (CancellationType.SelectedIndex)
				{
					case 0:
						newCancellationType = "Not Cxl";
						break;
					case 1:
						newCancellationType = "No Show";
						break;
					case 2:
						newCancellationType = "Late Cxl";
						break;
					case 3:
						newCancellationType = "Cxl";
						break;
					default:
						newCancellationType = "Not Cxl";
						break;
				}

				//Update All Changes
				var newExpense = (from exp in db.Expenses
								  where exp.ExpenseID == Session.ExpenseID
								  select exp).First();
				var newAppointmentID = newExpense.AppointmentID;
				newExpense.ExpenseTypeID = newExpenseTypeID;
				newExpense.ExpenseDueDate = newExpenseDueDate;
				decimal temp = 0;
				Decimal.TryParse(Copay.Text, out temp);
				newExpense.PatientBill = temp;
				decimal temp2 = 0;
				Decimal.TryParse(Deduction.Text, out temp2);
				newExpense.DonorBill = temp2;
				newExpense.TotalExpenseAmount = temp + temp2;
				if (IsPaid.IsChecked.Value == true)
				{
					newExpense.ExpensePaidDate = Convert.ToDateTime(ExpensePaidDate.ToString());
				}
				else
				{
					newExpense.ExpensePaidDate = null;
				}
				var newAppointment = (from a in db.Appointments
									  where a.AppointmentID == newAppointmentID
									  select a).First();
				newAppointment.StaffID = newStaffID;
				newAppointment.AppointmentStartDate = newStartDateTime;
				newAppointment.AppointmentEndDate = newEndDateTime;
				newAppointment.AppointmentCancelationType = newCancellationType;
				db.SaveChanges();
				this.Close();
			}
			catch (Exception error)
			{
				MessageBox.Show("Something went wrong. Please check the fields and try again.");
			}
		}

		private void Delete_Expense(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Session?",
			   "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();

				var expense = (from exp in db.Expenses
							   where exp.ExpenseID == Session.ExpenseID
							   select exp).First();

				var expenses = (from exp in db.Expenses
								where exp.AppointmentID == expense.AppointmentID
								select exp).ToList();
				int? DonationID = expense.DonationID;
				int? AppointmentID = expense.AppointmentID;

				db.Expenses.Remove(expense);

				//Add money back to the donation
				if (expense.DonorBill > 0)
				{
					var donation = (from d in db.Donations
									where d.DonationID == DonationID
									select d).First();
					donation.DonationAmountRemaining = donation.DonationAmountRemaining + expense.DonorBill;
					db.SaveChanges();
				}
				if (expenses.Count == 1)
				{
					var appt = (from d in db.Appointments
								where d.AppointmentID == AppointmentID
								select d).First();
					db.Appointments.Remove(appt);
					db.SaveChanges();
				}
				db.SaveChanges();

				//MessageBox.Show("This session has been deleted.");
				this.Close();
			}
		}

		private void Close_Window(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Hour_LostFocus(object sender, RoutedEventArgs e)
		{
			var textbox = sender as TextBox;
			textbox.Text = DisplayHourConversion(textbox.Text);
		}

		private void Minute_LostFocus(object sender, RoutedEventArgs e)
		{
			var textbox = sender as TextBox;
			textbox.Text = DisplayMinuteConversion(textbox.Text);
		}

		private string DisplayHourConversion(string hourIn)
		{
			try
			{
				int value = Convert.ToInt32(hourIn);

				if (value > 12)
					return "12";
				else if (value < 1)
					return "1";
				else
					return hourIn;
			}
			catch (Exception error)
			{
				return "12";
			}
		}

		private string DisplayMinuteConversion(string minuteIn)
		{
			try
			{
				int value = Convert.ToInt32(minuteIn);
				if (minuteIn.Length == 1)
					return "0" + minuteIn;
				else if (value > 59)
					return "59";
				else if (value < 0)
					return "00";
				else
					return minuteIn;
			}
			catch (Exception error)
			{
				return "00";
			}
		}

		private void Change_Paid_Bill(object sender, RoutedEventArgs e)
		{
			if (IsPaid.IsChecked.Value)
			{
				ExpensePaidDate.IsEnabled = true;
			}
			else
			{
				ExpensePaidDate.IsEnabled = false;
			}
		}

		private void Change_Client_Bill(object sender, RoutedEventArgs e)
		{
			if (DonorDeduction.IsChecked.Value)
			{
				MoneyDonation.Visibility = Visibility.Hidden;
				Grant.Visibility = Visibility.Visible;
			}
			else
			{
				MoneyDonation.Visibility = Visibility.Visible;
				Grant.Visibility = Visibility.Hidden;
			}
		}
	}
}
