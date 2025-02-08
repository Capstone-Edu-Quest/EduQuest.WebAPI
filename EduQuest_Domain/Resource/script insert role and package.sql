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

INSERT INTO [Edu_Quest].[dbo].[User] 
    ([Id], [Username], [AvatarUrl], [Email], [Phone], [Headline], [Status], 
     [Description], [RoleId], [PackagePrivilegeId], [AccountPackageId], 
     [CreatedAt], [UpdatedBy], [UpdatedAt], [DeletedAt])
VALUES
    (1, 'john_doe', 'https://example.com/avatar1.png', 'john.doe@example.com', '1234567890', 
     'Software Engineer', 'Active', 'Passionate developer with 5 years of experience.', 
     3, 1, 1, GETDATE(), 'Admin', GETDATE(), NULL),

    (2, 'jane_smith', 'https://example.com/avatar2.png', 'jane.smith@example.com', '0987654321', 
     'Data Scientist', 'Active', 'Experienced data scientist specialized in AI and ML.', 
     2, 2, 2, GETDATE(), 'Admin', GETDATE(), NULL),

    (3, 'alex_nguyen', 'https://example.com/avatar3.png', 'alex.nguyen@example.com', '1122334455', 
     'UX Designer', 'Inactive', 'Creative UI/UX designer with a passion for user experience.', 
     3, 1, 1, GETDATE(), 'Admin', GETDATE(), NULL);
