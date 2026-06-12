CREATE TABLE [Users].[PermissionRole]
(
    [RoleId] INTEGER NOT NULL,
    [PermissionsId] INTEGER NOT NULL,
    CONSTRAINT "PK_PermissionRole" PRIMARY KEY ("RoleId", "PermissionsId"),
    CONSTRAINT "FK_PermissionRole_Role" FOREIGN KEY ("RoleId") REFERENCES [Users].[Role](Id) ON DELETE CASCADE,
    CONSTRAINT "FK_PermissionRole_Permission" FOREIGN KEY ("PermissionsId") REFERENCES [Users].[Permissions](IDPermission) ON DELETE CASCADE
)
