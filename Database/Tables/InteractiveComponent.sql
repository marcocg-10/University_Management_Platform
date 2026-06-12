/*
Creates the [InteractiveComponent] table in the [InteractiveComponents] schema.

Columns:
- [Id]                  : Primary key. Auto-incremented identifier for each interactive component.
- [PlateId]             : Unique external identifier for the component.
- [Color]               : Color attribute of the component.
- [Texture]             : Surface texture description.
- [CoordinateX/Y/Z]     : 3D coordinates for the component’s position.
- [Width], [Height], [Depth] : Physical dimensions of the component.
- [XAxisRotation], [YAxisRotation], [ZAxisRotation] : Rotations of the component.
- [LearningSpaceId]    : Foreign key referencing the associated learning space.
*/
CREATE TABLE [InteractiveComponents].[InteractiveComponent]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PlateId] NVARCHAR(100) NOT NULL UNIQUE,
    [Color] NVARCHAR(50) NOT NULL,
    [Texture] NVARCHAR(100) NOT NULL,
    [CoordinateX] FLOAT NOT NULL,
    [CoordinateY] FLOAT NOT NULL,
    [CoordinateZ] FLOAT NOT NULL,
    [Width] FLOAT NOT NULL,
    [Height] FLOAT NOT NULL,
    [Depth] FLOAT NOT NULL,
    [XAxisRotation] FLOAT NOT NULL,
    [YAxisRotation] FLOAT NOT NULL,
    [ZAxisRotation] FLOAT NOT NULL,
    [LearningSpaceId] INT NOT NULL,

    -- Foreign key constraint
    CONSTRAINT FK_InteractiveComponent_LearningSpace 
        FOREIGN KEY ([LearningSpaceId]) REFERENCES [LearningSpaces].[LearningSpace]([Id])
);
