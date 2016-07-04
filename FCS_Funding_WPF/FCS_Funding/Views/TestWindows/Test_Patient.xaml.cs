using FCS_Funding.Models;
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

namespace FCS_Funding.Views.TestWindows
{
	/// <summary>
	/// Interaction logic for Test_Patient.xaml
	/// </summary>
	public partial class Test_Patient : Window
	{
		public Test_Patient()
		{
			InitializeComponent();
		}

		private void button_GetPatient_Click(object sender, RoutedEventArgs e)
		{
			string patientOQ = text_PatientOQ.Text;
			try
			{
				FCS_DBModel db = new FCS_DBModel();
				var patient = (from p in db.Patients
							   where p.PatientOQ == patientOQ
							   select p).First();

				text_AgeGroup.Text = patient.PatientAgeGroup;
				text_Ethnicity.Text = patient.PatientEthnicity;
				text_FirstName.Text = patient.PatientFirstName;
				text_Gender.Text = patient.PatientGender;
				text_LastName.Text = patient.PatientLastName;
				text_RelationToHEad.Text = patient.RelationToHead;
				check_IsHead.IsChecked = patient.IsHead;
			}
			catch
			{

			}
		}

		private void button_DeletePatient_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string patientOQ = text_PatientOQ.Text;
				FCS_DBModel db = new FCS_DBModel();
				int patID = db.Patients.Where(x => x.PatientOQ == patientOQ).Select(x => x.PatientID).Distinct().First();
				var patient = (from p in db.Patients
							   where p.PatientID == patID
							   select p).First();
				var patProblems = (from p in db.PatientProblems where p.PatientID == patID select p);
				db.Patients.Remove(patient);
				foreach (var item in patProblems)
				{
					db.PatientProblems.Remove(item);
				}
				db.SaveChanges();
			}
			catch { }
		}
	}
}
