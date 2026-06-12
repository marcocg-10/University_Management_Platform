CREATE TABLE [Users].[RoleUser]
(
    [RolesId] INTEGER NOT NULL,
    [UserIdKey] INTEGER NOT NULL,
    CONSTRAINT "PK_RoleUser" PRIMARY KEY ("RolesId", "UserIdKey"),
    CONSTRAINT "FK_RoleUser_Role" FOREIGN KEY ("RolesId") REFERENCES [Users].[Role](Id) ON DELETE CASCADE,
    CONSTRAINT "FK_RoleUser_User" FOREIGN KEY ("UserIdKey") REFERENCES [Users].[User](IdKey) ON DELETE CASCADE
)
