namespace FCS_Funding.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FCS_FundingDBModel : DbContext
    {
        public FCS_FundingDBModel()
            : base("name=FCS_FundingDBModel")
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<DonationPurpose> DonationPurposes { get; set; }
        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<DonorContact> DonorContacts { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<FundRaisingEvent> FundRaisingEvents { get; set; }
        public virtual DbSet<GrantProposal> GrantProposals { get; set; }
        public virtual DbSet<In_Kind_Item> In_Kind_Item { get; set; }
        public virtual DbSet<In_Kind_Service> In_Kind_Service { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientHousehold> PatientHouseholds { get; set; }
        public virtual DbSet<PatientProblem> PatientProblems { get; set; }
        public virtual DbSet<Problem> Problems { get; set; }
        public virtual DbSet<Purpose> Purposes { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<View_FamilySessionCount> View_FamilySessionCount { get; set; }
        public virtual DbSet<View_GrantProposals> View_GrantProposals { get; set; }
        public virtual DbSet<View_GroupSessionCount> View_GroupSessionCount { get; set; }
        public virtual DbSet<View_GroupSessionPreCount> View_GroupSessionPreCount { get; set; }
        public virtual DbSet<View_IndividualSessionCount> View_IndividualSessionCount { get; set; }
        public virtual DbSet<View_NewPatientByDoctor> View_NewPatientByDoctor { get; set; }
        public virtual DbSet<View_OnGoingPatientByDoctor> View_OnGoingPatientByDoctor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .Property(e => e.AppointmentCancelationType)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .HasMany(e => e.Expenses)
                .WithOptional(e => e.Appointment)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Donation>()
                .Property(e => e.DonationAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Donation>()
                .Property(e => e.DonationAmountRemaining)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Donation>()
                .HasMany(e => e.Expenses)
                .WithOptional(e => e.Donation)
                .WillCascadeOnDelete();

            modelBuilder.Entity<DonationPurpose>()
                .Property(e => e.DonationPurposeAmount)
                .HasPrecision(19, 4);

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
                .Property(e => e.DonorBill)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PatientBill)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Expense>()
                .Property(e => e.TotalExpenseAmount)
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

            modelBuilder.Entity<GrantProposal>()
                .Property(e => e.GrantStatus)
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

            modelBuilder.Entity<In_Kind_Service>()
                .Property(e => e.ServiceValue)
                .HasPrecision(19, 4);

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
                .Property(e => e.RelationToHead)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Expenses)
                .WithOptional(e => e.Patient)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PatientHousehold>()
                .Property(e => e.HouseholdIncomeBracket)
                .IsUnicode(false);

            modelBuilder.Entity<PatientHousehold>()
                .Property(e => e.HouseholdCounty)
                .IsUnicode(false);

            modelBuilder.Entity<Problem>()
                .Property(e => e.ProblemType)
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
                .Property(e => e.StaffUserName)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.StaffPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.StaffDBRole)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.Appointments)
                .WithRequired(e => e.Staff)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<View_FamilySessionCount>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_FamilySessionCount>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GrantProposals>()
                .Property(e => e.OrganizationName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GrantProposals>()
                .Property(e => e.GrantName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GrantProposals>()
                .Property(e => e.DonationAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<View_GroupSessionCount>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GroupSessionCount>()
                .Property(e => e.StaffLastName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GroupSessionPreCount>()
                .Property(e => e.StaffFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<View_GroupSessionPreCount>()
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
