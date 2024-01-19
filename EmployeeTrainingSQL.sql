
CREATE DATABASE EmployeeTrainingDB;

USE EmployeeTrainingDB;

--DROP DATABASE EmployeeTrainingDB

--Role Table--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--######################################################################################################################################################################################################
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Role') DROP TABLE Role
BEGIN
CREATE TABLE Role(
		RoleID					TINYINT			IDENTITY(1,1)		CONSTRAINT [PK_Role] PRIMARY KEY,
		RoleName				VARCHAR(50)		NOT NULL			UNIQUE  CHECK(RoleName IN ('Admin','Manager', 'Employee')),
		IsActive				BIT				NOT NULL			DEFAULT 1,

		CreateTimestamp		    DATETIME         NOT NULL            DEFAULT GETDATE(),
		LastModifiedUserId      SMALLINT         NULL,                                  
	    LastModifiedTimestamp   DATETIME         NULL,
);
END
ALTER TABLE Role
ADD CONSTRAINT CK_Role_RoleName CHECK(RoleName IN ('Admin', 'Manager', 'Employee', 'Trainer'));
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


--Department Table---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--######################################################################################################################################################################################################
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Department') DROP TABLE Department
BEGIN
CREATE TABLE Department(
		DepartmentID			TINYINT			IDENTITY(1,1)		CONSTRAINT [PK_Department] PRIMARY KEY,
		DepartmentName			NVARCHAR(200)	NOT NULL			UNIQUE, 
		IsActive				BIT				NOT NULL			DEFAULT 1,

		CreateTimestamp		    DATETIME         NOT NULL            DEFAULT GETDATE(),
		LastModifiedUserId      SMALLINT         NULL,
	    LastModifiedTimestamp   DATETIME         NULL,
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



--User Table 
----######################################################################################################################################################################################################
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User') DROP TABLE [User]
BEGIN
CREATE TABLE [User](
		UserID					SMALLINT		IDENTITY(1,1)		CONSTRAINT [PK_User] PRIMARY KEY,
		FirstName				NVARCHAR(100)	NOT NULL,
		LastName				NVARCHAR(100)	NOT NULL,			
		Email					VARCHAR(255)	NOT NULL			UNIQUE,
		NIC						VARCHAR(50)		NOT NULL			UNIQUE     CHECK (LEN(NIC) = 14),
		MobileNumber			VARCHAR(8)		NOT NULL			UNIQUE     CHECK (MobileNumber LIKE '5[0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
		DepartmentID			TINYINT			NULL				CONSTRAINT FK_Department_User REFERENCES Department(DepartmentID)		ON DELETE  SET NULL,
		ManagerID				SMALLINT		NULL				CONSTRAINT FK_Manager_User REFERENCES [User] (UserID),
		IsActive				BIT				NOT NULL			DEFAULT 1,

		CreateTimestamp		   DATETIME         NOT NULL            DEFAULT GETDATE(),
		LastModifiedUserId     SMALLINT         NULL                                     CONSTRAINT FK_User_LastModifiedUser REFERENCES [User] (UserID),
	    LastModifiedTimestamp  DATETIME         NULL,
);
END

-- Add foreign key constraint for LastModifiedUserId in Role Table
ALTER TABLE Role ADD CONSTRAINT FK_Role_LastModifiedUser FOREIGN KEY (LastModifiedUserId) REFERENCES [User] (UserID);

-- Add foreign key constraint for LastModifiedUserId in Department Table
ALTER TABLE Department ADD CONSTRAINT FK_Department_LastModifiedUser FOREIGN KEY (LastModifiedUserId) REFERENCES [User] (UserID);
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


---User Role----
----######################################################################################################################################################################################################
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserRole') DROP TABLE UserRole
CREATE TABLE UserRole(
		UserRoleID				INT					IDENTITY(1,1)	CONSTRAINT [PK_UserRole] PRIMARY KEY,
		UserID					SMALLINT							CONSTRAINT FK_UserRole_User REFERENCES [User] (UserID),
		RoleID					TINYINT			    DEFAULT 3      	CONSTRAINT FK_Role_User REFERENCES Role(RoleID),
);
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


--Account
-----######################################################################################################################################################################################################
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Account') DROP TABLE Account
BEGIN
CREATE TABLE Account(
		AccountID						SMALLINT					IDENTITY(1,1)			             CONSTRAINT [PK_Account] PRIMARY KEY,
		UserID							SMALLINT 				    NOT NULL				UNIQUE       CONSTRAINT FK_Account_User REFERENCES [User] (UserID),
		HashedPassword					VARBINARY(MAX)		            NOT NULL,
		SaltValue						VARBINARY(MAX)		        NOT NULL,
		IsActive						BIT				            NOT NULL			    DEFAULT 1,
		
		CreateTimestamp					DATETIME					NOT NULL				DEFAULT GETDATE(),
		LastModifiedUserId				SMALLINT					NULL                                       CONSTRAINT FK_Account_LastModifiedUser REFERENCES [User] (UserID),
	    LastModifiedTimestamp			DATETIME					NULL,
		LastLoginTimestamp          	DATETIME                    NULL,
		LastPasswordChangedTimestamp	DATETIME                    NULL,
		PasswordExpiryTimestamp         DATETIME                    NOT NULL				DEFAULT DATEADD(DAY, 90, GETDATE()), -- Default expiration date is set to 90 days from the current date,
		IsAccountLocked					BIT                         NOT NULL                DEFAULT 0,
		DefaultSessionTimeout			INT                         NOT NULL                DEFAULT 15,
		HasAcceptedPrivacyPolicy        BIT                         NOT NULL                DEFAULT 1,
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

----######################################################################################################################################################################################################
 CREATE TABLE PasswordHistory (
    HistoryID                   INT                        IDENTITY(1,1)            CONSTRAINT [PK_PasswordHistory]  PRIMARY KEY,
    UserID                      SMALLINT                                            FOREIGN KEY REFERENCES [User](UserID),
    OldHashedPassword			BINARY(64)		           NOT NULL,
    OldSaltValue				UniqueIdentifier	       NOT NULL,
    ChangeDate                  DATETIME                   NOT NULL                 DEFAULT GETDATE()
);
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


--Training Table
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Training') DROP TABLE Training
BEGIN
CREATE TABLE Training(
		TrainingID				TINYINT			IDENTITY(1,1)		CONSTRAINT [PK_Training] PRIMARY KEY,
		TrainingName			NVARCHAR(100)   NOT NULL			UNIQUE,
		RegistrationDeadline	DATE			NOT NULL,
		TrainingDescription		NVARCHAR(500)	NOT NULL,
		StartDate				DATE			NOT NULL,
		Capacity				TINYINT			NOT NULL,
		DepartmentID			TINYINT			NULL				CONSTRAINT FK_Department_Training REFERENCES Department(DepartmentID), 
		IsActive				BIT				NOT NULL			DEFAULT 1,

		CreateTimestamp					DATETIME					NOT NULL				DEFAULT GETDATE(),
		LastModifiedUserId				SMALLINT					NULL                                         CONSTRAINT FK_Training_LastModifiedUser REFERENCES [User] (UserID),
	    LastModifiedTimestamp			DATETIME					NULL,
);
END
ALTER TABLE Training
ADD IsSelectionOver   BIT NOT NULL DEFAULT 0;

ALTER TABLE Training
ADD LastSelectionTimeStamp         DATETIME NULL;

ALTER TABLE Training
ADD EndDate DATE NULL;

ALTER TABLE Training
ADD TrainerID SMALLINT NULL CONSTRAINT FK_Training_User REFERENCES [User] (UserID);


ALTER TABLE Training
ADD SelectedByUserID SMALLINT NULL CONSTRAINT FK_SeectionTraining_User REFERENCES [User] (UserID);

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



--Prerequisite
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Prerequisite') DROP TABLE Prerequisite
BEGIN
CREATE TABLE Prerequisite(
		PrerequisiteID			TINYINT			IDENTITY(1,1)		CONSTRAINT [PK_Prerequisite] PRIMARY KEY,
		PrerequisiteName		NVARCHAR(100)   NOT NULL			UNIQUE,		
		IsActive				BIT				NOT NULL			DEFAULT 1,

		CreateTimestamp					DATETIME					NOT NULL				DEFAULT GETDATE(),
		LastModifiedUserId				SMALLINT					NULL                    CONSTRAINT FK_Prerequisite_LastModifiedUser REFERENCES [User] (UserID),
	    LastModifiedTimestamp			DATETIME					NULL,
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



--Training Prerequisite
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TrainingPrerequisite') DROP TABLE TrainingPrerequisite
BEGIN
CREATE TABLE TrainingPrerequisite(
		TrainingPrerequisiteID  INT				IDENTITY(1,1)       CONSTRAINT [PK_TrainingPrerequisite] PRIMARY KEY, 
		TrainingID				TINYINT								CONSTRAINT FK_Training_Prerequisite REFERENCES Training(TrainingID), 
		PrerequisiteID			TINYINT 							CONSTRAINT FK_Prerequisite_Training REFERENCES Prerequisite(PrerequisiteID), 
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



--Enrollment
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enrollment') DROP TABLE Enrollment
BEGIN
CREATE TABLE Enrollment(
	EnrollmentID			SMALLINT		 IDENTITY(1,1)			CONSTRAINT [PK_Enrollment] PRIMARY KEY,
	UserID					SMALLINT								CONSTRAINT FK_Enrollment_User REFERENCES [User] (UserID),
	TrainingID				TINYINT									CONSTRAINT FK_Enrollment_Training REFERENCES Training(TrainingID),
	EnrollmentDate			DateTime		 DEFAULT GETDATE(), -- Set default to current date
	EnrollmentStatus		NVARCHAR(20)	 DEFAULT 'Pending'		CHECK(EnrollmentStatus IN ('Pending','Approved', 'Rejected')),
	DeclineReason			VARCHAR(255)	 NULL,                 ------PUT NVARHAR
	ApprovedByUserId		SMALLINT		 NULL                   CONSTRAINT FK_Enrollment_ApprovedByUser REFERENCES [User] (UserID),
    DeclinedByUserId		SMALLINT		 NULL                   CONSTRAINT FK_Enrollment_DeclinedByUser REFERENCES [User] (UserID),
	IsActive				BIT				 NOT NULL				DEFAULT 1,

	CreateTimestamp			DATETIME		 NOT NULL				DEFAULT GETDATE(),
	LastModifiedUserId		SMALLINT		 NULL                   CONSTRAINT FK_Enrollment_LastModifiedUser REFERENCES [User] (UserID),
	LastModifiedTimestamp	DATETIME		 NULL,
		
);
END
ALTER TABLE Enrollment
ADD IsSelected BIT NOT NULL DEFAULT 0;

---*************************************
ALTER TABLE Enrollment
ADD		FormerApprovedByUserId		SMALLINT		 NULL                   CONSTRAINT FK_Enrollment_FormerApprovedByUser REFERENCES [User] (UserID);
ALTER TABLE Enrollment
ADD     FormerDeclinedByUserId		SMALLINT		 NULL                   CONSTRAINT FK_Enrollment_FormerDeclinedByUser REFERENCES [User] (UserID);
ALTER TABLE Enrollment
ADD     FormerDeclinedReason	    VARCHAR(255)	 NULL;

ALTER TABLE Enrollment
ADD     SelectedTimestamp			DATETIME		 NULL;
ALTER TABLE Enrollment
ADD     ApprovedTimestamp			DATETIME		 NULL;
ALTER TABLE Enrollment
ADD     RejectedTimestamp			DATETIME		 NULL;
ALTER TABLE Enrollment
ADD     SoftDeleteTimestamp			DATETIME		 NULL;

ALTER TABLE Enrollment
ADD SelectedByUserID SMALLINT NULL CONSTRAINT FK_EnrollmentSelection_User REFERENCES [User] (UserID);
---****************************************
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




--Attachment
----######################################################################################################################################################################################################
--HARD DELETE--TO SAVE SPACE

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Attachment') DROP TABLE Attachment
BEGIN
CREATE TABLE Attachment(
		AttachmentID		     INT		     IDENTITY(1,1)		    CONSTRAINT [PK_Attachment] PRIMARY KEY,
		EnrollmentID			 SMALLINT		            			CONSTRAINT FK_Attachment_Enrollment REFERENCES Enrollment(EnrollmentID),
		PrerequisiteID			 TINYINT 								CONSTRAINT FK_Attachment_Prerequisite REFERENCES Prerequisite(PrerequisiteID),
		FileName		         NVARCHAR(200)	 NOT NULL,
		AttachmentURL			 NVARCHAR(200)	 NULL,
		FileData				 VARBINARY(MAX)  NOT NULL,
		FileSize				 INT             NULL,

		CreateTimestamp					DATETIME					NOT NULL				DEFAULT GETDATE(),
		LastModifiedUserId				SMALLINT					NULL                    CONSTRAINT FK_Attachment_LastModifiedUser REFERENCES [User] (UserID),
	    LastModifiedTimestamp			DATETIME					NULL,
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



--Permision
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Permission') DROP TABLE Permission
BEGIN
CREATE TABLE Permission(
		PermissionID					TINYINT			IDENTITY(1,1)		CONSTRAINT [PK_Permission] PRIMARY KEY,
		PermissionName				    VARCHAR(50)		NOT NULL			UNIQUE,
		IsActive				        BIT				NOT NULL			DEFAULT 1,

		CreateTimestamp					DATETIME         NOT NULL            DEFAULT GETDATE(),
		LastModifiedUserId				SMALLINT         NULL,                                  
	    LastModifiedTimestamp			DATETIME         NULL,
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



--Role Permision
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RolePermission') DROP TABLE RolePermission
BEGIN
CREATE TABLE RolePermission(
		RolePermissionID					TINYINT			IDENTITY(1,1)		CONSTRAINT [PK_RolePermission] PRIMARY KEY,
		RoleID					            TINYINT			NOT NULL			CONSTRAINT FK_RolePermissin_Role REFERENCES Role(RoleID),
		PermissionID					    TINYINT		NOT NULL			CONSTRAINT FK_RolePermission_Permisssion REFERENCES Permission (PermissionID),
);
END
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





-- Notification Table
----######################################################################################################################################################################################################

IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AppNotification') DROP TABLE Notification
BEGIN
CREATE TABLE AppNotification(
    AppNotificationID       INT               IDENTITY(1,1)       CONSTRAINT [PK_AppNotification] PRIMARY KEY,
    UserID                  SMALLINT          NOT NULL            CONSTRAINT FK_AppNotificationn_User REFERENCES [User] (UserID),
	EnrollmentID			SMALLINT		  NULL          		  CONSTRAINT FK_AppNotification_Enrollment REFERENCES Enrollment(EnrollmentID),
	NotificationSubject     NVARCHAR(100)     NOT NULL,
    NotificationMessage     NVARCHAR(500)     NULL,
    Status                  NVARCHAR(50)      NULL,
    HasRead                 BIT               NOT NULL            DEFAULT 0,

    CreateTimestamp         DATETIME          NOT NULL            DEFAULT GETDATE(),
    LastModifiedUserId      SMALLINT          NULL                CONSTRAINT FK_Notification_LastModifiedUser REFERENCES [User] (UserID),
    LastModifiedTimestamp   DATETIME          NULL,
);
END
ALTER TABLE AppNotification
ADD NotificationSender NVARCHAR(50) NULL;
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



------USER ACTIVITY TABLE
----######################################################################################################################################################################################################
CREATE TABLE UserActivity (
    UserActivityID          INT            IDENTITY(1,1)     CONSTRAINT [PK_UserActivity] PRIMARY KEY,
    UserID                  SMALLINT       NOT NULL          CONSTRAINT FK_UserActivity_User REFERENCES [User] (UserID),
    CurrentRole             VARCHAR(50)    NULL,
    UrlVisited              NVARCHAR(255)  NULL,
    HttpMethod              VARCHAR(10)    NULL,
    ActionParameters        NVARCHAR(MAX)  NULL,
    IpAddress               VARCHAR(50)    NULL,
    UrlVisitedTimestamp     DATETIME       NULL,
    UserAgent               NVARCHAR(255)  NULL,
    SessionID               NVARCHAR(255)  NULL,
    Referer                 NVARCHAR(255)  NULL,
    StatusCode              INT            NULL,
    UserLocation            NVARCHAR(255)  NULL,
    IsMobileDevice          BIT            NULL,
);
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




---SessionHistory
----######################################################################################################################################################################################################
CREATE TABLE SessionHistory (
    SessionHistoryID INT IDENTITY(1,1)		CONSTRAINT [PK_SessionHistory] PRIMARY KEY,
    UserID			SMALLINT NOT NULL		CONSTRAINT FK_SessionHistory_User REFERENCES [User] (UserID),
    EventType		NVARCHAR(10) NOT NULL, --   'Login' or 'Logout'
    EventTime		DATETIME NOT NULL,
    IPAddress		NVARCHAR(50) NULL,
    UserAgent		NVARCHAR(255) NULL,
    IsMobileDevice  BIT NULL,
    UserLocation	NVARCHAR(255) NULL,
    SessionID		NVARCHAR(255) NULL,
    Referer			NVARCHAR(255) NULL
);
