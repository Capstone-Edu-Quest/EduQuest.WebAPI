-- Insert data into PackagePrivilege table
INSERT INTO [Edu_Quest].[dbo].[PackagePrivilege] ([Id], [Name], [Description], [CreatedAt], [UpdatedBy], [UpdatedAt], [DeletedAt])
VALUES 
(1, 'Basic Privilege', 'Access to basic features', GETDATE(), 'Admin', GETDATE(), NULL),
(2, 'Advanced Privilege', 'Access to advanced features', GETDATE(), 'Admin', GETDATE(), NULL);

-- Insert data into AccountPackage table
INSERT INTO [Edu_Quest].[dbo].[AccountPackage] ([Id], [Name], [Description], [DurationDays], [Price], [IsFree], [CreatedAt], [UpdatedBy], [UpdatedAt], [DeletedAt])
VALUES 
(1, 'Free Plan', 'Basic plan with limited access', 30, 0, 1, GETDATE(), 'Admin', GETDATE(), NULL),
(2, 'Premium Plan', 'Full access to all features', 365, 100, 0, GETDATE(), 'Admin', GETDATE(), NULL);

-- Insert data into Role table
INSERT INTO [Edu_Quest].[dbo].[Role] ([Id], [RoleName], [CreatedAt], [UpdatedBy], [UpdatedAt], [DeletedAt])
VALUES 
(1, 'Admin', GETDATE(), 'System', GETDATE(), NULL),
(2, 'Instructor', GETDATE(), 'System', GETDATE(), NULL),
(3, 'Learner', GETDATE(), 'System', GETDATE(), NULL),
(4, 'Guest', GETDATE(), 'System', GETDATE(), NULL);

INSERT INTO [Edu_Quest].[dbo].[User] (
    [Id],
    [Email],
    [Phone],
    [UpdatedAt],
    [CreatedAt],
    [Username],
    [RoleId],
    [UpdatedBy],
    [DeletedAt],
    [AccountPackageId],
    [AvatarUrl],
    [PackagePrivilegeId],
    [Description],
    [Headline],
	Status
)
VALUES 
-- Example User 1
(1, 
 'user1@example.com', 
 '1234567890', 
 GETDATE(), 
 GETDATE(), 
 'user1', 
 1, 
 'Admin', 
 NULL, 
 1, 
 'https://example.com/avatar1.jpg', 
 1, 
 'This is a description for User 1.', 
 'User 1 Headline',
 'active'
),
-- Example User 2
(2, 
 'user2@example.com', 
 '0987654321', 
 GETDATE(), 
 GETDATE(), 
 'user2', 
 2, 
 'Admin', 
 NULL, 
 2, 
 'https://example.com/avatar2.jpg', 
 2, 
 'This is a description for User 2.', 
 'User 2 Headline',
 'active'
);

