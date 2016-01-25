DROP DATABASE IF EXISTS FCS_Funding;
CREATE DATABASE FCS_Funding;

USE FCS_Funding;

CREATE TABLE Donation
(
	DonationID				INT				NOT NULL 		AUTO_INCREMENT 			PRIMARY KEY,
    EventID					INT				NULL,
    DonorID					INT				NOT NULL,
    RequestForPersonalID	INT				NULL,
    Restricted				Bool	    	NOT NULL,
    InKind					bool			NOT NULL,
    DonationAmount			decimal(20,2)	NOT NULL,
    DonationDate			date			NOT NULL
);

CREATE TABLE In_Kind_Service
(
	ServiceID				INT				NOT NULL 		AUTO_INCREMENT			PRIMARY KEY,
    DonationID				INT				NOT NULL,
    StartDateTime			datetime		NOT NULL,
    EndDateTime				datetime		NOT NULL,
    RatePerHour				Decimal(10,2)	NOT NULL,
    ServiceDescription		VarChar(5000)	NOT NULL
);

CREATE TABLE In_Kind_Item
(
	ItemID					INT				NOT NULL 			AUTO_INCREMENT			PRIMARY KEY,
    DonationID				INT				NOT NULL,
    ItemName				VarChar(50)		NOT NULL,
    /*DateDonated			DATE			NOT NULL,*/
    ItemDescription			VarChar(5000)	NOT NULL
);

CREATE TABLE FundRaisingEvent
(
	EventID					INT				NOT NULL 		AUTO_INCREMENT			PRIMARY KEY,
    EventStartDateTime		datetime		NOT NULL,
    EventEndDateTime		datetime		NOT NULL,
    EventName				VarChar(50)		NOT NULL,
    EventDescription		VarChar(5000)	NULL
);

CREATE TABLE Expense
(
	ExpenseID				INT				NOT NULL 		AUTO_INCREMENT			PRIMARY KEY,
	ExpenseTypeID			INT				NOT NULL,
    DonationID				INT				NOT NULL,
    PatientID				INT				NOT NULL,
    ExpenseDueDate			Date			NOT NULL,
    ExpensePaidDate			Date			NOT NULL,
    ExpenseAmount			Decimal(10,2)	NOT NULL
);

CREATE TABLE DonationPurpose
(
	DonationPurposeID		INT				NOT NULL 		AUTO_INCREMENT			PRIMARY KEY,
    DonationID				INT				NOT NULL,
    PurposeID				INT				NOT NULL,
    DonationPurposeAmount	Decimal(10,2)	NOT NULL
);

CREATE TABLE Donor
(
	DonorID					INT				NOT NULL 		AUTO_INCREMENT			PRIMARY KEY,
    DonorFirstName			VarChar(50)		NOT NULL,
    DonorLastName			VarChar(50)		NOT NULL,
    OrganizationName		VarChar(250)	NULL,
    DonorAddress1			VarChar(50)		NULL,
    DonorAddress2			VarChar(50)		NULL,
    DonorState				VarChar(2)		NULL,
    DonorCity				VarChar(200)	NULL,
    DonorZip				INT				NULL
);

CREATE TABLE Reminder
(
	ReminderID				INT				NOT NULL 		AUTO_INCREMENT			PRIMARY KEY,
    DonorID					INT				NOT NULL,
    ReminderDate			datetime		NOT NULL,
    ReminderDescription				VarChar(5000)	NULL
);

CREATE TABLE DonorContact
(
	ContactID				INT				NOT NULL		AUTO_INCREMENT			PRIMARY KEY,
    DonorID					INT				NOT NULL,
    ContactFirstName		VarChar(30)		NOT NULL,
    ContactLastName			VarChar(30)		NULL,
    ContactPhone			VarChar(10)		NULL,
    ContactEmail			VarChar(700)	NULL
);

CREATE TABLE GrantProposal
(
	GrantProposalID			INT				NOT NULL		AUTO_INCREMENT			PRIMARY KEY,
    DonorID					INT				NOT NULL,
    GrantName				VarChar(50)		NOT NULL,
    SubmissionDueDate		date			NOT NULL
);

CREATE TABLE Purpose
(
	PurposeID				INT				NOT NULL		AUTO_INCREMENT			PRIMARY KEY,
    PurposeName				VarChar(100)	NOT NULL,
    PurposeDescription		VarChar(5000)	NOT NULL
);

CREATE TABLE Patient
(
	PatientID				INT				NOT NULL		AUTO_INCREMENT			PRIMARY KEY,
    PatienOQ				INT				NOT NULL,
    PatientFirstName		VarChar(50)		NOT NULL,
    PatientLastName			VarChar(50)		NOT NULL
);

CREATE TABLE ExpenseType
(
	ExpenseTypeID			INT				NOT NULL		AUTO_INCREMENT			PRIMARY KEY,
    ExpenseShortDescription	VarChar(50)		NOT NULL,
    ExpenseLongDescription	VarChar(5000)	NULL
);


ALTER TABLE Donation
	ADD
	CONSTRAINT FK_Donation_DonorID
    FOREIGN KEY(DonorID) REFERENCES Donor(DonorID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE In_Kind_Item
	ADD
	CONSTRAINT FK_Item_DonationID
    FOREIGN KEY(DonationID) REFERENCES Donation(DonationID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE In_Kind_Service
	ADD
	CONSTRAINT FK_Service_DonationID
    FOREIGN KEY(DonationID) REFERENCES Donation(DonationID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE Expense
	ADD
	CONSTRAINT FK_Expense_ExpenseTypeID
    FOREIGN KEY(ExpenseTypeID) REFERENCES ExpenseType(ExpenseTypeID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE Expense
	ADD
	CONSTRAINT FK_Expense_DonationID
    FOREIGN KEY(DonationID) REFERENCES Donation(DonationID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE Expense
	ADD
	CONSTRAINT FK_Expense_PatientID
    FOREIGN KEY(PatientID) REFERENCES Patient(PatientID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE DonationPurpose
	ADD 
    CONSTRAINT FK_DonationPurpose_DonationID
    FOREIGN KEY(DonationID) REFERENCES Donation(DonationID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE DonationPurpose
	ADD 
    CONSTRAINT FK_DonationPurpose_PurposeID
    FOREIGN KEY(PurposeID) REFERENCES Purpose(PurposeID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;
    
ALTER TABLE Reminder
	ADD 
    CONSTRAINT FK_Reminder_DonorID
    FOREIGN KEY(DonorID) REFERENCES Donor(DonorID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE DonorContact
	ADD 
    CONSTRAINT FK_DonorContact_DonorID
    FOREIGN KEY(DonorID) REFERENCES Donor(DonorID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE GrantProposal
	ADD 
    CONSTRAINT FK_GrantProposal_DonorID
    FOREIGN KEY(DonorID) REFERENCES Donor(DonorID)
    ON UPDATE CASCADE
    ON DELETE CASCADE;

