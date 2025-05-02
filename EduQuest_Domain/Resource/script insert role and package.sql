

-- Insert data into Role table
INSERT INTO public."Role" ("Id", "RoleName", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
VALUES
(1, 'Admin', NOW(), 'System', NOW(), NULL),
(2, 'Instructor', NOW(), 'System', NOW(), NULL),
(3, 'Learner', NOW(), 'System', NOW(), NULL),
(4, 'Guest', NOW(), 'System', NOW(), NULL),
(5, 'Expert', NOW(), 'System', NOW(), NULL),
(6, 'Staff', NOW(), 'System', NOW(), NULL);

INSERT INTO public."SystemConfig" ("Id", "Name", "Value", "Description", "CreatedAt", "UpdatedAt")
VALUES
    ('1', 'Video', 1.0, 'Configuration for video', NOW(), NOW()),
    ('2', 'Document', 1.0, 'Configuration for document', NOW(), NOW()),
    ('3', 'Quiz', 0.8, 'Configuration for quiz', NOW(), NOW()),
    ('4', 'Assignment', 1.0, 'Configuration for assignment', NOW(), NOW());



-- Insert Instructor Pro - Price Monthly
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (3, '2', 'Pro', 'priceMonthly', 9, NOW(), NULL, NULL, NULL);

-- Insert Instructor Pro - Price Yearly
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (4, '2', 'Pro', 'priceYearly', 90, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Price Monthly
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (5, '3', 'Pro', 'priceMonthly', 5, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Price Yearly
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (6, '3', 'Pro', 'priceYearly', 50, NOW(), NULL, NULL, NULL);

-- Insert Instructor Free - Benefit Commission Fee
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (7, '2', 'Free', 'commissionFee', 18, NOW(), NULL, NULL, NULL);

-- Insert Instructor Pro - Benefit Commission Fee
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (8, '2', 'Pro', 'commissionFee', 12, NOW(), NULL, NULL, NULL);

-- Insert Instructor Pro - Benefit Marketing Email Per Month
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (9, '2', 'Pro', 'marketingEmailPerMonth', 3, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Benefit Coupon Per Month
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (10, '3', 'Pro', 'couponPerMonth', 3, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Benefit Coupon Discount Upto
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (11, '3', 'Pro', 'couponDiscountUpto', 90, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Benefit Extra Gold and Exp
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (12, '3', 'Pro', 'extraGoldAndExp', 10, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Benefit Trial Course Percentage
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (13, '3', 'Pro', 'trialCoursePercentage', 15, NOW(), NULL, NULL, NULL);

-- Insert Learner Pro - Benefit Course Trial Per Month
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (14, '3', 'Pro', 'courseTrialPerMonth', 5, NOW(), NULL, NULL, NULL);

INSERT INTO public."Tag" ("Id", "Name", "Level", "Grade", "Type") VALUES
(1, 'Math', 1, 1, 'Expertise'),
(2, 'Math', 1, 2, 'Expertise'),
(3, 'Math', 1, 3, 'Expertise'),
(4, 'Math', 1, 4, 'Expertise'),
(5, 'Math', 1, 5, 'Expertise'),
(6, 'Vietnamese', 1, 1, 'Expertise'),
(7, 'Vietnamese', 1, 2, 'Expertise'),
(8, 'Vietnamese', 1, 3, 'Expertise'),
(9, 'Vietnamese', 1, 4, 'Expertise'),
(10, 'Vietnamese', 1, 5, 'Expertise'),
(11, 'English', 1, 3, 'Expertise'),
(12, 'English', 1, 4, 'Expertise'),
(13, 'English', 1, 5, 'Expertise'),
(14, 'Math', 2, 6, 'Expertise'),
(15, 'Math', 2, 7, 'Expertise'),
(16, 'Math', 2, 8, 'Expertise'),
(17, 'Math', 2, 9, 'Expertise'),
(18, 'Literature', 2, 6, 'Expertise'),
(19, 'Literature', 2, 7, 'Expertise'),
(20, 'Literature', 2, 8, 'Expertise'),
(21, 'Literature', 2, 9, 'Expertise'),
(22, 'English', 2, 6, 'Expertise'),
(23, 'English', 2, 7, 'Expertise'),
(24, 'English', 2, 8, 'Expertise'),
(25, 'English', 2, 9, 'Expertise'),
(26, 'Physics', 2, 6, 'Expertise'),
(27, 'Physics', 2, 7, 'Expertise'),
(28, 'Physics', 2, 8, 'Expertise'),
(29, 'Physics', 2, 9, 'Expertise'),
(30, 'Chemistry', 2, 8, 'Expertise'),
(31, 'Chemistry', 2, 9, 'Expertise'),
(32, 'Biology', 2, 6, 'Expertise'),
(33, 'Biology', 2, 7, 'Expertise'),
(34, 'Biology', 2, 8, 'Expertise'),
(35, 'Biology', 2, 9, 'Expertise'),
(36, 'History', 2, 6, 'Expertise'),
(37, 'History', 2, 7, 'Expertise'),
(38, 'History', 2, 8, 'Expertise'),
(39, 'History', 2, 9, 'Expertise'),
(40, 'Geography', 2, 6, 'Expertise'),
(41, 'Geography', 2, 7, 'Expertise'),
(42, 'Geography', 2, 8, 'Expertise'),
(43, 'Geography', 2, 9, 'Expertise'),
(44, 'Math', 3, 10, 'Expertise'),
(45, 'Math', 3, 11, 'Expertise'),
(46, 'Math', 3, 12, 'Expertise'),
(47, 'Literature', 3, 10, 'Expertise'),
(48, 'Literature', 3, 11, 'Expertise'),
(49, 'Literature', 3, 12, 'Expertise'),
(50, 'English', 3, 10, 'Expertise'),
(51, 'English', 3, 11, 'Expertise'),
(52, 'English', 3, 12, 'Expertise'),
(53, 'Physics', 3, 10, 'Expertise'),
(54, 'Physics', 3, 11, 'Expertise'),
(55, 'Physics', 3, 12, 'Expertise'),
(56, 'Chemistry', 3, 10, 'Expertise'),
(57, 'Chemistry', 3, 11, 'Expertise'),
(58, 'Chemistry', 3, 12, 'Expertise'),
(59, 'Biology', 3, 10, 'Expertise'),
(60, 'Biology', 3, 11, 'Expertise'),
(61, 'Biology', 3, 12, 'Expertise'),
(62, 'History', 3, 10, 'Expertise'),
(63, 'History', 3, 11, 'Expertise'),
(64, 'History', 3, 12, 'Expertise'),
(65, 'Geography', 3, 10, 'Expertise'),
(66, 'Geography', 3, 11, 'Expertise'),
(67, 'Geography', 3, 12, 'Expertise');
