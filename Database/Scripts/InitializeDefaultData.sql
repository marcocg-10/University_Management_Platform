/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Master script to initialize all default data
-- This script runs after database deployment

-- 1. Insert Permissions (using MERGE to avoid duplicates)
MERGE INTO [Users].[Permissions] AS Target
USING (VALUES 
    ('ListUsers'),
    ('CreateUsers'),
    ('AssignRole'),
    ('ManageRoles'),
    ('ManageBuildings'),
    ('ManageInterComponents'),
    ('ManageLearningSpaces'),
    ('ListBuildings'),
    ('ListInterComponents'),
    ('ListLearningSpaces')
) AS Source ([Name])
ON Target.[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Name])
    VALUES (Source.[Name]);

-- 2. Insert Roles (using MERGE to avoid duplicates)
MERGE INTO [Users].[Role] AS Target
USING (VALUES 
    ('UserAdmin'),
    ('BuildingAdmin'),
    ('InterComponentsAdmin'),
    ('LearningSpacesAdmin'),
    ('SuperAdmin'),
    ('Guest')
) AS Source ([Name])
ON Target.[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Name])
    VALUES (Source.[Name]);

-- 3. Insert Super Admin User (using MERGE to avoid duplicates)
MERGE INTO [Users].[User] AS Target
USING (VALUES 
    ('sa0001', 'Super Admin', 'themeparkatucr@gmail.com', 1, '0c515227-bfd0-45af-ae46-8e3de1d9d305')
) AS Source ([IdUser], [Name], [Email], [IsActive], [AzureObjectIdentifier])
ON Target.[IdUser] = Source.[IdUser]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([IdUser], [Name], [Email], [IsActive], [AzureObjectIdentifier])
    VALUES (Source.[IdUser], Source.[Name], Source.[Email], Source.[IsActive], Source.[AzureObjectIdentifier]);

-- 4. Insert Role-Permission relationships for SuperAdmin
-- Get the SuperAdmin role ID and all permission IDs
DECLARE @SuperAdminRoleId INT;
SELECT @SuperAdminRoleId = [Id] FROM [Users].[Role] WHERE [Name] = 'SuperAdmin';

MERGE INTO [Users].[PermissionRole] AS Target
USING (
    SELECT [IDPermission] AS PermissionsId, @SuperAdminRoleId AS RoleId
    FROM [Users].[Permissions]
) AS Source
ON Target.[RoleId] = Source.[RoleId] AND Target.[PermissionsId] = Source.[PermissionsId]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([RoleId], [PermissionsId])
    VALUES (Source.[RoleId], Source.[PermissionsId]);

DECLARE @GuestRoleId INT;
SELECT @GuestRoleId = [Id] FROM [Users].[Role] WHERE [Name] = 'Guest';

MERGE INTO [Users].[PermissionRole] AS Target
USING (
    SELECT [IDPermission] AS PermissionsId, @GuestRoleId AS RoleId
    FROM [Users].[Permissions]
    WHERE [Name] IN ('ListBuildings', 'ListInterComponents', 'ListLearningSpaces')
) AS Source
ON Target.[RoleId] = Source.[RoleId] AND Target.[PermissionsId] = Source.[PermissionsId]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([RoleId], [PermissionsId])
    VALUES (Source.[RoleId], Source.[PermissionsId]);

-- 5. Insert User-Role relationship for Super Admin
DECLARE @SuperAdminUserId INT;
SELECT @SuperAdminUserId = [IdKey] FROM [Users].[User] WHERE [IdUser] = 'sa0001';

MERGE INTO [Users].[RoleUser] AS Target
USING (
    SELECT @SuperAdminRoleId AS RolesId, @SuperAdminUserId AS UserIdKey
) AS Source
ON Target.[RolesId] = Source.[RolesId] AND Target.[UserIdKey] = Source.[UserIdKey]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([RolesId], [UserIdKey])
    VALUES (Source.[RolesId], Source.[UserIdKey]);

--------------------------------------------------------------------------------------
-- 6. Insert default LearningSpaceTexture values
--------------------------------------------------------------------------------------
MERGE INTO [LearningSpaces].[LearningSpaceTexture] AS Target
USING (VALUES
    ('Outdoor_Wall_T01_Roughness.png'),
    ('Outdoor_Wall_T02_Height.png'),
    ('Outdoor_Wall_T10_Ambient_occlusion.png')
) AS Source ([Texture])
ON Target.[Texture] = Source.[Texture]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Texture])
    VALUES (Source.[Texture]);
