namespace FCS_Funding.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FCS_DB : DbContext
    {
        public FCS_DB()
            : base("name=FCS_DB1")
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Donation> Donation { get; set; }
        public virtual DbSet<DonationPurpose> DonationPurpose { get; set; }
        public virtual DbSet<Donor> Donor { get; set; }
        public virtual DbSet<DonorContact> DonorContact { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<FundRaisingEvent> FundRaisingEvent { get; set; }
        public virtual DbSet<GrantProposal> GrantProposal { get; set; }
        public virtual DbSet<In_Kind_Item> In_Kind_Item { get; set; }
        public virtual DbSet<In_Kind_Service> In_Kind_Service { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientHousehold> PatientHousehold { get; set; }
        public virtual DbSet<PatientProblem> PatientProblem { get; set; }
        public virtual DbSet<Purpose> Purpose { get; set; }
        public virtual DbSet<Reminder> Reminder { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<View_FamilySessionCount> View_FamilySessionCount { get; set; }
        public virtual DbSet<View_GroupSessionCount> View_GroupSessionCount { get; set; }
        public virtual DbSet<View_IndividualSessionCount> View_IndividualSessionCount { get; set; }
        public virtual DbSet<View_NewPatientByDoctor> View_NewPatientByDoctor { get; set; }
        public virtual DbSet<View_OnGoingPatientByDoctor> View_OnGoingPatientByDoctor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .Property(e => e.AppointmentCancelationType)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .HasMany(e => e.Expense)
                .WithOptional(e => e.Appointment)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Donation>()
                .Property(e => e.DonationAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Donation>()
                .HasMany(e => e.Expense)
                .WithOptional(e => e.Donation)
                .WillCascadeOnDelete();

            modelBuilder.Entity<DonationPurpose>()
                .Property(e => e.DonationPurposeAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorType)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.OrganizationName)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorState)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorCity)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.DonorZip)
                .IsUnicode(false);

            modelBuilder.Entity<DonorContact>()
                .Property(e => e.ContactFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<DonorContact>()
                .Property(e => e.ContactLastName)
                .IsUnicode(false);

            modelBuilder.Entity<DonorContact>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<DonorContact>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.ExpenseAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ExpenseType>()
                .Property(e => e.ExpenseType1)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseType>()
                .Property(e => e.ExpenseDescription)
                .IsUnicode(false);

            modelBuilder.Entity<FundRaisingEvent>()
                .Property(e => e.EventName)
                .IsUnicode(false);

            modelBuilder.Entity<FundRaisingEvent>()
                .Property(e => e.EventDescription)
                .IsUnicode(false);

            modelBuilder.Entity<GrantProposal>()
                .Property(e => e.GrantName)
                .IsUnicode(false);

            modelBuilder.Entity<In_Kind_Item>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<In_Kind_Item>()
                .Property(e => e.ItemDescription)
                .IsUnicode(false);

            modelBuilder.Entity<In_Kind_Service>()
                .Property(e => e.RatePerHour)
                .HasPrecision(19, 4);

            modelBuilder.Entity<In_Kind_Service>()
                .Property(e => e.ServiceDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.PatientFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.PatientLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.PatientGender)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.PatientAgeGroup)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.PatientEthnicity)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Expense)
                .WithOptional(e => e.Patient)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PatientHousehold>()
                .Property(e => e.HouseholdIncomeBracket)
                .IsUnicode(false);

            modelBuilder.Entity<PatientHousehold>()
                .Property(e => e.HouseholdCounty)
                .IsUnicode(false);

            modelBuilder.Entity<PatientProblem>()
                .Property(e => e.ProbelmType)
                .IsUnicode(false);

            modelBuilder.Entity<Purpose>()
                .Property(e => e.PurposeName)
                .IsUnicode(false);

            modelBuilder.Entity<Purpose>()
                .Property(e => e.PurposeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Reminder>()
                .Property(e => e.ReminderDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.StaffTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.Appointment)
                .WithRequired(e => e.Staff)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<View_FamilySessionCount>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_FamilySessionCount>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GroupSessionCount>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GroupSessionCount>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<View_IndividualSessionCount>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_IndividualSessionCount>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<View_NewPatientByDoctor>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_NewPatientByDoctor>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<View_OnGoingPatientByDoctor>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_OnGoingPatientByDoctor>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);
        }
    }
}
