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
using FCS_Funding.Views;
using FCS_DataTesting;
using System.Collections.ObjectModel;
using FCS_Funding.Models;
using System.Data;
using System.Windows.Forms;

namespace FCS_Funding.Views.UserControls
{
	/// <summary>
	/// Interaction logic for Report_GrantDonations.xaml
	/// </summary>
	public partial class Report_GrantDonations : System.Windows.Controls.UserControl
	{
		FCS_DBModel db = new FCS_DBModel();

		//public List<Grant> GrantList = new List<Grant>();
		//public List<Session> GrantSessionList = new List<Session>();
		public ObservableCollection<GrantsDataGrid> Grants { get; set; }
		public ObservableCollection<SessionsGrid> Sessions { get; set; }

		public int GID { get; set; }
		public Report_GrantDonations()
		{
			InitializeComponent();
		}
		public class Grant
		{
			public String GrantName { get; set; }
			public String DonationAmount { get; set; }
			public String DonationAmountRemaining { get; set; }
			public String DonationDate { get; set; }
			public String DonationExpirationDate { get; set; }
			public String DonationID { get; set; }
			public String DonorID { get; set; }
			public String GrantProposalID { get; set; }
		};
		public class Session
		{
			public String ClientName { get; set; }
			public int Sessions { get; set; }
			public decimal Amount { get; set; }
			public DateTime Date { get; set; }
			public decimal Balance { get; set; }

		};


		private void Grant_DataGridReport_Loaded(object sender, RoutedEventArgs e)
		{
			//db = new FCS_DBModel();
			var join = from d in db.Donations
					   join dr in db.Donors on d.DonorID equals dr.DonorID
					   join gp in db.GrantProposals on dr.DonorID equals gp.DonorID
					   where gp.GrantStatus == "Accepted" && d.GrantProposalID == gp.GrantProposalID
					   select new GrantsDataGrid
					   {
						   GrantName = gp.GrantName,
						   DonationAmount = d.DonationAmount,
						   DonationAmountRemaining = d.DonationAmountRemaining,
						   DonationDate = d.DonationDate,
						   ExpirationDate = d.DonationExpirationDate,
						   DonationID = d.DonationID,
						   DonorID = dr.DonorID,
						   GrantProposalID = gp.GrantProposalID
					   };


			var grid = sender as System.Windows.Controls.DataGrid;
			grid.ItemsSource = join.ToList();
			int currentYear = DateTime.Now.Year;
			DateTime firstDay = new DateTime(currentYear, 1, 1);
			DateTime lastDay = new DateTime(currentYear, 12, 31);
			if (grantReportFrom_datepicker.SelectedDate == null || grantReportFrom_datepicker.SelectedDate.GetValueOrDefault() <= DateTime.MinValue)
			{
				grantReportFrom_datepicker.SelectedDate = firstDay;
			}
			if (grantReportTo_datepicker.SelectedDate == null || grantReportTo_datepicker.SelectedDate.GetValueOrDefault() < DateTime.MinValue)
			{
				grantReportTo_datepicker.SelectedDate = lastDay;
			}
		}

		private void getSessionData()
		{
			List<Session> GrantSessionList = new List<Session>();
			int GID = 0;
			//db = new FCS_DBModel();
			System.Windows.Controls.DataGrid dg = Grant_DataGridReport as System.Windows.Controls.DataGrid;
			if (dg.SelectedItems.Count != 0)
			{
				GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				label2.Content = "List of sessions for: " + p.GrantName;
				GID = p.GrantProposalID;

				if (grantReportFrom_datepicker.SelectedDate != null && grantReportFrom_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue && grantReportTo_datepicker.SelectedDate != null && grantReportTo_datepicker.SelectedDate.GetValueOrDefault() != DateTime.MinValue && GID != 0)
				{
					DateTime requestedDateStart = (DateTime)grantReportFrom_datepicker.SelectedDate;
					DateTime requestedDateEnd = (DateTime)grantReportTo_datepicker.SelectedDate;

					decimal remainingBalance = p.DonationAmount;
					var join = from g in db.GrantProposals
							   join d in db.Donations on g.GrantProposalID equals d.GrantProposalID
							   join ex in db.Expenses on d.DonationID equals ex.DonationID
							   join pa in db.Patients on ex.PatientID equals pa.PatientID
							   join ap in db.Appointments on ex.AppointmentID equals ap.AppointmentID
							   where g.GrantProposalID == GID && ap.AppointmentEndDate > requestedDateStart && ap.AppointmentEndDate < requestedDateEnd
							   orderby ap.AppointmentEndDate
							   select new SessionsDataGrid
							   {
								   ClientName = pa.PatientLastName + ", " + pa.PatientFirstName,
								   Sessions = 1,
								   Amount = ex.DonorBill,
								   Date = ap.AppointmentEndDate,
								   Balance = 0  //All balance calculations are done after collection is generated
							   };

					//Clear list of previous sessions
					GrantSessionList.Clear();

					//Loop over join collection generated above to calculate the correct remaining balance for each row
					foreach (var joint in join)
					{
						remainingBalance = remainingBalance - joint.Amount;
						GrantSessionList.Add(new Session()
						{
							ClientName = joint.ClientName,
							Sessions = joint.Sessions,
							Amount = joint.Amount,
							Date = joint.Date,
							Balance = remainingBalance
						});
						GrantSessionList.AddRange(GrantSessionList);
						GrantSessionList.AddRange(GrantSessionList);
						GrantSessionList.AddRange(GrantSessionList);

					}
				}
			}
			GrantSessions_DataGridReport.ItemsSource = GrantSessionList;
			//gGrantSessionList = GrantSessionList;
		}
		private void Grant_DataGridReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			getSessionData();
		}
		private void grantReportTo_datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			getSessionData();
		}

		private void Grant_DataGridReport_MouseDown(object sender, MouseButtonEventArgs e)
		{
			getSessionData();
		}

		private void grantReportFrom_datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			getSessionData();
		}

		private void generate_Report()
		{
			System.Windows.Controls.DataGrid dg = Grant_DataGridReport as System.Windows.Controls.DataGrid;
			if (dg.SelectedItems.Count != 0)
			{
				GrantsDataGrid p = (GrantsDataGrid)dg.SelectedItems[0];
				System.Windows.Forms.WebBrowser newBrowser = new System.Windows.Forms.WebBrowser();
				newBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintDocument);
				int pCount = 1;
				int rCount = 1;
				String browserString = "<!DOCTYPE html>"
				+ "<html>"
				+ "<head>"
				+ "<style>"
				+ "header nav, footer {"
				+ "    display: none;"
				+ "}"
				+ "body {"
				+ "    white-space: nowrap;"
				+ "}"
				+ "div.titlepage {"
				+ "  page: blank; "
				+ "}"
				+ ".table {"
				+ "   margin-top: 50px;"
				+ "}"
				+ ".break {"
				+ "   page-break-after: always;"
				+ "}"
				+ ".cell {"
				+ "   font-size:20px;"
				+ "   text-align: left;"
				+ "   border: 1px solid black;"
				+ "}"
				+ ".left {"
				+ "   color:#000000;"
				+ "   font-weight:bold;"
				+ "   font-size:20px;"
				+ "   text-align: left;"
				+ "}"
				+ ".right {"
				+ "   color:#000000;"
				+ "   font-weight:bold;"
				+ "   font-size:20px;"
				+ "   text-align: right;"
				+ "}"
				+ ".center {"
				+ "   color:#FF0000;"
				+ "   font-weight:bold;"
				+ "   font-size:20px;"
				+ "   text-align: center;"
				+ "}"
				+ "</style>"
				+ "</head>"
				+ "<body>";
				System.Windows.Controls.DataGrid ds = GrantSessions_DataGridReport as System.Windows.Controls.DataGrid;
				int itemCount = GrantSessions_DataGridReport.Items.Count;
				if (itemCount == 0)
				{
					System.Windows.MessageBox.Show("There are no sessions to print for this Grant/Donation.");
				}
				else
				{
					foreach (Session row in GrantSessions_DataGridReport.Items)
					{
						if (rCount == 1)
						{
							if (pCount == 1)
							{
								browserString += "    <div class='left' width='1229px' style='margin-top:10px;'>Grant Name: " + p.GrantName + "</div>"
								+ "    <div class='left' width='1229px' style='margin-top:10px;'>Amount Received: " + Math.Round(p.DonationAmount, 2).ToString() + "</div>"
								+ "    <div class='left' width='1229px' style='margin-top:10px;'>Date: " + p.DonationDate.ToString("MM/dd/yyyy") + "</div>"
								+ "    <div class='left' width='1229px' style='margin-top:10px;'>Description: " + p.PurposeDescription + "</div>"
								+ "    <div class='right' width='1229px' style='margin-top:10px;'>Grant Total: " + Math.Round(p.DonationAmountRemaining, 2).ToString() + "</div>";
							}
							else
							{
								browserString += "    <div class='left' width='1229px' style='margin-top:10px;'>Grant Name: " + p.GrantName + "</div>";

							}

							browserString += "    <div class='table'><table width='100%' border='0.5' bordercolor='black' bgcolor='black'>"
							+ "        <tr border='0'>"
							+ "            <td border='0.5' style='width:656px; padding-left: 4px; text-align: center;' bgcolor='white' bordercolor='black' class='cell'>Client Name</td>"
							+ "            <td border='0.5' style='width:95px; padding-left: 4px; text-align: center;' bgcolor='white' bordercolor='black' class='cell'>Sessions</td>"
							+ "            <td border='0.5' style='width:91px; padding-left: 4px; text-align: center;' bgcolor='white' bordercolor='black' class='cell'>Amount</td>"
							+ "            <td border='0.5' style='width:91px; padding-left: 4px; text-align: center;' bgcolor='white' bordercolor='black' class='cell'>Date</td>"
							+ "            <td border='0.5' style='width:96px; padding-left: 4px; text-align: center;' bgcolor='white' bordercolor='black' class='cell'>Balance</td>"
							+ "        </tr>";
						}
						browserString += "        <tr>"
						+ "            <td style='width:656px; padding-left: 4px;' bgcolor='white' bordercolor='black' class='cell'>" + row.ClientName + "</td>"
						+ "            <td style='width:95px; padding-left: 4px;' bgcolor='white' bordercolor='black' class='cell'>" + row.Sessions + "</td>"
						+ "            <td style='width:91px; padding-left: 4px;' bgcolor='white' bordercolor='black' class='cell'>" + Math.Round(row.Amount, 2) + "</td>"
						+ "            <td style='width:91px; padding-left: 4px;' bgcolor='white' bordercolor='black' class='cell'>" + row.Date.ToString("MM/dd/yyyy") + "</td>"
						+ "            <td style='width:96px; padding-left: 4px;' bgcolor='white' bordercolor='black' class='cell'>" + Math.Round(row.Balance, 2) + "</td>"
						+ "        </tr>";
						if ((pCount == 1 && rCount % 22 == 0) || rCount == itemCount + 1 || (pCount >= 2 && rCount % 27 == 0))
						{
							browserString += "    </table></div>";
							if (pCount != rCount)
							{
								browserString += "    <div class='break'></div>";
							}
							rCount = 1;
							pCount++;
						}
						else
						{
							rCount++;
						}
					}
					browserString += "</body>"
					+ "</html>";
					newBrowser.DocumentText = browserString;
				}
			}
		}
		private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			((System.Windows.Forms.WebBrowser)sender).ShowPageSetupDialog();
			//((System.Windows.Forms.WebBrowser)sender).Print();
			((System.Windows.Forms.WebBrowser)sender).ShowPrintPreviewDialog();

		}

		private void printGrantSessions_button_Click(object sender, RoutedEventArgs e)
		{
			generate_Report();
		}
	}
}
