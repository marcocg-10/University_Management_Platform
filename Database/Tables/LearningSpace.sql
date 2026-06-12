CREATE TABLE [LearningSpaces].[LearningSpace]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[BuildingId] INT NULL,  -- Can be NULL if it is not associated with a building.
	[FloorLevel] INT NULL,  -- Can be NULL if it is not associated with a floor in a building.
	[RoomId] NVARCHAR(50) NOT NULL,  -- Not primary key, e.g., "3-5", "Room A".
	[Color] NVARCHAR(10) NOT NULL DEFAULT '#CDCECF',
    [Texture] NVARCHAR(50) NOT NULL
        CONSTRAINT [DF_LearningSpace_Texture] DEFAULT (N'Outdoor_Wall_T01_Roughness.png'),
	[Length] REAL NOT NULL,
	[Width] REAL NOT NULL,
	[Height] REAL NOT NULL,
	[XCoordinate] REAL NOT NULL,
	[YCoordinate] REAL NOT NULL,
	[ZCoordinate] REAL NOT NULL,
	CONSTRAINT PK_LearningSpace PRIMARY KEY ([Id])
)
GO

-- Add a unique constraint to ensure no two rooms in the same building have the same RoomId.
ALTER TABLE [LearningSpaces].[LearningSpace]
ADD CONSTRAINT UNIQUE_Room_Building
UNIQUE ([RoomId], [BuildingId])
GO

-- Add Foreign Key constraints to Building table once they exist.
ALTER TABLE [LearningSpaces].[LearningSpace]
ADD CONSTRAINT FK_LearningSpace_Building 
FOREIGN KEY ([BuildingId]) 
REFERENCES [Buildings].[Building]([Id]); 
GO

-- Add Foreign Key constraints to LearningSpaceTexture table once they exist.
ALTER TABLE [LearningSpaces].[LearningSpace]
ADD CONSTRAINT FK_LearningSpace_Texture 
FOREIGN KEY ([Texture]) 
REFERENCES [LearningSpaces].[LearningSpaceTexture]([Texture]);
GO
