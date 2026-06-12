CREATE TABLE [LearningSpaces].[Classroom]
(
	[Id] INT NOT NULL
	CONSTRAINT PK_Classroom PRIMARY KEY
)
GO

-- Add Foreign Key constraint to LearningSpace table (TPT).
ALTER TABLE [LearningSpaces].[Classroom]
ADD CONSTRAINT FK_Classroom_LearningSpace
FOREIGN KEY ([Id]) 
REFERENCES [LearningSpaces].[LearningSpace]([Id])
ON DELETE CASCADE;

