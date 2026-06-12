/*
Creates the [Board] table in the [InteractiveComponents] schema.

Columns:
- [Id]: Primary key. Auto-incremented identifier for each interactive component.
- [MarkerColor]: Color attribute of the marker used on the Board.
*/
CREATE TABLE [InteractiveComponents].[Board]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [MarkerColor] NVARCHAR(50) NOT NULL

    CONSTRAINT FK_Board_InteractiveComponent 
        FOREIGN KEY ([Id]) REFERENCES [InteractiveComponents].[InteractiveComponent]([Id])
);
