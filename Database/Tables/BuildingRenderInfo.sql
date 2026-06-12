CREATE TABLE buildings.BuildingRenderInfo (
    Id INT NOT NULL IDENTITY(1,1),
    Depth DECIMAL(18,2) NOT NULL,
    Width DECIMAL(18,2) NOT NULL,
    Height DECIMAL(18,2) NOT NULL,
    Color NVARCHAR(10) NOT NULL DEFAULT '#CDCECF',
    Texture NVARCHAR(50) NOT NULL DEFAULT 'Outdoor_Wall_T03_Ambient_occlusion.png',
    X DECIMAL(18,2) NOT NULL,
    Y DECIMAL(18,2) NOT NULL,
    Z DECIMAL(18,2) NOT NULL,
    BuildingId INT NOT NULL,
    CONSTRAINT PK_BuildingRenderInfo PRIMARY KEY(id),
    CONSTRAINT FK_BuildingRenderInfo_Building FOREIGN KEY(BuildingId) REFERENCES buildings.Building(Id)
);
