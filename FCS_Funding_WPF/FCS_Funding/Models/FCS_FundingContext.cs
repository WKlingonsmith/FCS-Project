using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using FCS_Funding.Models.Mapping;

namespace FCS_Funding.Models
{
    public partial class FCS_FundingContext : DbContext
    {
        static FCS_FundingContext()
        {
            Database.SetInitializer<FCS_FundingContext>(null);
        }

        public FCS_FundingContext()
            : base("Name=FCS_FundingContext")
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationPurpose> DonationPurposes { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonorContact> DonorContacts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<FundRaisingEvent> FundRaisingEvents { get; set; }
        public DbSet<GrantProposal> GrantProposals { get; set; }
        public DbSet<In_Kind_Item> In_Kind_Item { get; set; }
        public DbSet<In_Kind_Service> In_Kind_Service { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientHousehold> PatientHouseholds { get; set; }
        public DbSet<PatientProblem> PatientProblems { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Purpose> Purposes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<View_FamilySessionCount> View_FamilySessionCount { get; set; }
        public DbSet<View_GroupSessionCount> View_GroupSessionCount { get; set; }
        public DbSet<View_GroupSessionPreCount> View_GroupSessionPreCount { get; set; }
        public DbSet<View_IndividualSessionCount> View_IndividualSessionCount { get; set; }
        public DbSet<View_NewPatientByDoctor> View_NewPatientByDoctor { get; set; }
        public DbSet<View_OnGoingPatientByDoctor> View_OnGoingPatientByDoctor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AppointmentMap());
            modelBuilder.Configurations.Add(new DonationMap());
            modelBuilder.Configurations.Add(new DonationPurposeMap());
            modelBuilder.Configurations.Add(new DonorMap());
            modelBuilder.Configurations.Add(new DonorContactMap());
            modelBuilder.Configurations.Add(new ExpenseMap());
            modelBuilder.Configurations.Add(new ExpenseTypeMap());
            modelBuilder.Configurations.Add(new FundRaisingEventMap());
            modelBuilder.Configurations.Add(new GrantProposalMap());
            modelBuilder.Configurations.Add(new In_Kind_ItemMap());
            modelBuilder.Configurations.Add(new In_Kind_ServiceMap());
            modelBuilder.Configurations.Add(new PatientMap());
            modelBuilder.Configurations.Add(new PatientHouseholdMap());
            modelBuilder.Configurations.Add(new PatientProblemMap());
            modelBuilder.Configurations.Add(new ProblemMap());
            modelBuilder.Configurations.Add(new PurposeMap());
            modelBuilder.Configurations.Add(new ReminderMap());
            modelBuilder.Configurations.Add(new StaffMap());
            modelBuilder.Configurations.Add(new View_FamilySessionCountMap());
            modelBuilder.Configurations.Add(new View_GroupSessionCountMap());
            modelBuilder.Configurations.Add(new View_GroupSessionPreCountMap());
            modelBuilder.Configurations.Add(new View_IndividualSessionCountMap());
            modelBuilder.Configurations.Add(new View_NewPatientByDoctorMap());
            modelBuilder.Configurations.Add(new View_OnGoingPatientByDoctorMap());
        }
    }
}
