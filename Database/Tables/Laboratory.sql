CREATE TABLE [LearningSpaces].[Laboratory]
(
	[Id] INT NOT NULL
	CONSTRAINT PK_Laboratory PRIMARY KEY
)
GO

-- Add Foreign Key constraint to LearningSpace table (TPT).
ALTER TABLE [LearningSpaces].[Laboratory]
ADD CONSTRAINT FK_Laboratory_LearningSpace
FOREIGN KEY ([Id]) 
REFERENCES [LearningSpaces].[LearningSpace]([Id])
ON DELETE CASCADE;
