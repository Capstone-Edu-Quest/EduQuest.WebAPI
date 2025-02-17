-- Insert data into PackagePrivilege table
INSERT INTO public."PackagePrivilege" ("Id", "Name", "Description", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
VALUES 
(1, 'Basic Privilege', 'Access to basic features', NOW(), 'Admin', NOW(), NULL),
(2, 'Advanced Privilege', 'Access to advanced features', NOW(), 'Admin', NOW(), NULL);

-- Insert data into AccountPackage table
INSERT INTO public."AccountPackage" ("Id", "Name", "Description", "DurationDays", "Price", "IsFree", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
VALUES 
(1, 'Free Plan', 'Basic plan with limited access', 30, 0, TRUE, NOW(), 'Admin', NOW(), NULL),
(2, 'Premium Plan', 'Full access to all features', 365, 100, FALSE, NOW(), 'Admin', NOW(), NULL);

-- Insert data into Role table
INSERT INTO public."Role" ("Id", "RoleName", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
VALUES 
(1, 'Admin', NOW(), 'System', NOW(), NULL),
(2, 'Instructor', NOW(), 'System', NOW(), NULL),
(3, 'Learner', NOW(), 'System', NOW(), NULL),
(4, 'Guest', NOW(), 'System', NOW(), NULL),
(5, 'Staff', NOW(), 'System', NOW(), NULL),
(6, 'Expert', NOW(), 'System', NOW(), NULL);

INSERT INTO public."SystemConfig" ("Id", "Name", "Value", "Description")
VALUES 
    (1, 'Docs', 1.0, 'Configuration for documents'),
    (2, 'Video', 1.0, 'Configuration for videos'),
    (3, 'Quiz', 0.8, 'Configuration for quizzes');
