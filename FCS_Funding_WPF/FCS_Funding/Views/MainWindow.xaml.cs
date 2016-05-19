using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FCS_Funding.Views;
using FCS_DataTesting;
using System.Collections.ObjectModel;
using FCS_Funding.Models;
using System.Windows.Data;

namespace FCS_Funding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PatientGrid> Patients { get; set; }
        public ObservableCollection<GrantsDataGrid> Grants { get; set; }
        public ObservableCollection<DonorsDataGrid> Donors { get; set; }
        public ObservableCollection<InKindItem> InKindItems { get; set; }
        public ObservableCollection<InKindService> InKindServices { get; set; }
        public ObservableCollection<EventsDataGrid> Events { get; set; }
        public ObservableCollection<ReportsDataGrid> Reports { get; set; }
        public ObservableCollection<AdminDataGrid> Admins { get; set; }
        public string PatientsSearchByEnum { get; set; }

        //Accessablity
        public string StaffDBRole { get; set; }

        public MainWindow(string StaffRole)
        {
            StaffDBRole = StaffRole;
            //DGrid.ItemsSource = data;
            InitializeComponent();
        }

		private void CreateBindingsForTabs(string roleForStaff)
		{
			Binding b = new Binding();
			b.Source = roleForStaff;
			b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			b.Path = new PropertyPath("StaffRole");
			
		}

        private void Create_Backup(object sender, RoutedEventArgs e)
        {
            //if (Application.Current.Windows.Count <= 1)
            //{
            //    CreateBackup cb = new CreateBackup();
            //    cb.Show();
            //}
        }
    }
}