/*
Creates the [Projector] table in the [InteractiveComponents] schema.

Columns:
- [Id]: Primary key. Auto-incremented identifier for each interactive component.
- [Brightness]: Brightness level of the projector.
- [ResolutionWidth]: Width of the projector's resolution in pixels.
- [ResolutionHeight]: Height of the projector's resolution in pixels.
*/
CREATE TABLE [InteractiveComponents].[Projector]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [Brightness] INT NOT NULL,
    [ResolutionWidth] INT NOT NULL,
    [ResolutionHeight] INT NOT NULL

    CONSTRAINT FK_Projector_InteractiveComponent 
        FOREIGN KEY ([Id]) REFERENCES [InteractiveComponents].[InteractiveComponent]([Id])
);
