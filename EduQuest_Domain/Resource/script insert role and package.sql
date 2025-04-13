

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
