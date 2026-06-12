CREATE TABLE [Users].[User] (
  [IdKey] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
  [IdUser] NVARCHAR(30) NOT NULL UNIQUE,
  --should be alphanumeric because could be and id from costa rica (cédula) or student id 
  [Name] NVARCHAR(50) NOT NULL,
  [IsActive] BIT NOT NULL DEFAULT 1,
  [Email] NVARCHAR(100) NOT NULL UNIQUE,
  [AzureObjectIdentifier] NVARCHAR(36),
  [AvatarId] NVARCHAR(50),

  CONSTRAINT CK_User_NameLength CHECK (LEN([Name]) >= 3),
  CONSTRAINT CK_User_IDLength CHECK (LEN([IdUser]) >= 5),
  CONSTRAINT CK_User_Email CHECK ([Email] LIKE '%@%'),
);