

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
(1, 'Toán', 1, 1, 'Expertise'),
(2, 'Toán', 1, 2, 'Expertise'),
(3, 'Toán', 1, 3, 'Expertise'),
(4, 'Toán', 1, 4, 'Expertise'),
(5, 'Toán', 1, 5, 'Expertise'),
(6, 'Tiếng Việt', 1, 1, 'Expertise'),
(7, 'Tiếng Việt', 1, 2, 'Expertise'),
(8, 'Tiếng Việt', 1, 3, 'Expertise'),
(9, 'Tiếng Việt', 1, 4, 'Expertise'),
(10, 'Tiếng Việt', 1, 5, 'Expertise'),
(11, 'Tiếng Anh', 1, 3, 'Expertise'),
(12, 'Tiếng Anh', 1, 4, 'Expertise'),
(13, 'Tiếng Anh', 1, 5, 'Expertise'),
(14, 'Toán', 2, 6, 'Expertise'),
(15, 'Toán', 2, 7, 'Expertise'),
(16, 'Toán', 2, 8, 'Expertise'),
(17, 'Toán', 2, 9, 'Expertise'),
(18, 'Ngữ văn', 2, 6, 'Expertise'),
(19, 'Ngữ văn', 2, 7, 'Expertise'),
(20, 'Ngữ văn', 2, 8, 'Expertise'),
(21, 'Ngữ văn', 2, 9, 'Expertise'),
(22, 'Tiếng Anh', 2, 6, 'Expertise'),
(23, 'Tiếng Anh', 2, 7, 'Expertise'),
(24, 'Tiếng Anh', 2, 8, 'Expertise'),
(25, 'Tiếng Anh', 2, 9, 'Expertise'),
(26, 'Vật lý', 2, 6, 'Expertise'),
(27, 'Vật lý', 2, 7, 'Expertise'),
(28, 'Vật lý', 2, 8, 'Expertise'),
(29, 'Vật lý', 2, 9, 'Expertise'),
(30, 'Hóa học', 2, 8, 'Expertise'),
(31, 'Hóa học', 2, 9, 'Expertise'),
(32, 'Sinh học', 2, 6, 'Expertise'),
(33, 'Sinh học', 2, 7, 'Expertise'),
(34, 'Sinh học', 2, 8, 'Expertise'),
(35, 'Sinh học', 2, 9, 'Expertise'),
(36, 'Lịch sử', 2, 6, 'Expertise'),
(37, 'Lịch sử', 2, 7, 'Expertise'),
(38, 'Lịch sử', 2, 8, 'Expertise'),
(39, 'Lịch sử', 2, 9, 'Expertise'),
(40, 'Địa lý', 2, 6, 'Expertise'),
(41, 'Địa lý', 2, 7, 'Expertise'),
(42, 'Địa lý', 2, 8, 'Expertise'),
(43, 'Địa lý', 2, 9, 'Expertise'),
(44, 'Toán', 3, 10, 'Expertise'),
(45, 'Toán', 3, 11, 'Expertise'),
(46, 'Toán', 3, 12, 'Expertise'),
(47, 'Ngữ văn', 3, 10, 'Expertise'),
(48, 'Ngữ văn', 3, 11, 'Expertise'),
(49, 'Ngữ văn', 3, 12, 'Expertise'),
(50, 'Tiếng Anh', 3, 10, 'Expertise'),
(51, 'Tiếng Anh', 3, 11, 'Expertise'),
(52, 'Tiếng Anh', 3, 12, 'Expertise'),
(53, 'Vật lý', 3, 10, 'Expertise'),
(54, 'Vật lý', 3, 11, 'Expertise'),
(55, 'Vật lý', 3, 12, 'Expertise'),
(56, 'Hóa học', 3, 10, 'Expertise'),
(57, 'Hóa học', 3, 11, 'Expertise'),
(58, 'Hóa học', 3, 12, 'Expertise'),
(59, 'Sinh học', 3, 10, 'Expertise'),
(60, 'Sinh học', 3, 11, 'Expertise'),
(61, 'Sinh học', 3, 12, 'Expertise'),
(62, 'Lịch sử', 3, 10, 'Expertise'),
(63, 'Lịch sử', 3, 11, 'Expertise'),
(64, 'Lịch sử', 3, 12, 'Expertise'),
(65, 'Địa lý', 3, 10, 'Expertise'),
(66, 'Địa lý', 3, 11, 'Expertise'),
(67, 'Địa lý', 3, 12, 'Expertise');

