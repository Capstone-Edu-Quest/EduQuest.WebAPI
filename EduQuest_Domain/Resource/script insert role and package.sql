

-- Insert data into Role table
INSERT INTO public."Role" ("Id", "RoleName", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
VALUES
(1, 'Admin', NOW(), 'System', NOW(), NULL),
(2, 'Instructor', NOW(), 'System', NOW(), NULL),
(3, 'Learner', NOW(), 'System', NOW(), NULL),
(4, 'Guest', NOW(), 'System', NOW(), NULL),
(5, 'Staff', NOW(), 'System', NOW(), NULL),
(6, 'Expert', NOW(), 'System', NOW(), NULL);

INSERT INTO public."SystemConfig" ("Id", "Name", "Value", "Description", "CreatedAt", "UpdatedAt")
VALUES
    ('1', 'Video', 1.0, 'Configuration for video', NOW(), NOW()),
    ('2', 'Document', 1.0, 'Configuration for document', NOW(), NOW()),
    ('3', 'Quiz', 0.8, 'Configuration for quiz', NOW(), NOW()),
    ('4', 'Assignment', 1.0, 'Configuration for assignment', NOW(), NOW());


INSERT INTO public."Course"(
    "Id", "Title", "Description", "PhotoUrl", "Color", "Price", 
    "Requirement", "Feature", "IsRequired", "Status", "CreatedBy", 
    "AdvertiseId", "CourseLearnerId", "SettingId", "CreatedAt", 
    "UpdatedBy", "UpdatedAt", "DeletedAt"
)
VALUES
    ('3', 'Course 1', 'Description of Course 1', 'https://example.com/photo1.jpg', 'Red', 199.99, 'Basic knowledge of programming', 'Feature 1', true, 'Active', '98dd3a59-a722-4d2d-822b-2a967cc08df6', NULL, NULL, NULL, NOW(), 'Admin', NOW(), NULL),
    ('4', 'Course 2', 'Description of Course 2', 'https://example.com/photo2.jpg', 'Blue', 299.99, 'Intermediate level knowledge of programming', 'Feature 2', false, 'Active', '98dd3a59-a722-4d2d-822b-2a967cc08df6', NULL, NULL, NULL, NOW(), 'Admin', NOW(), NULL),
    ('5', 'Course 3', 'Description of Course 3', 'https://example.com/photo3.jpg', 'Green', 149.99, 'No prior knowledge required', 'Feature 3', true, 'Inactive', '98dd3a59-a722-4d2d-822b-2a967cc08df6', NULL, NULL, NULL, NOW(), 'Admin', NOW(), NULL),
    ('6', 'Course 4', 'Description of Course 4', 'https://example.com/photo4.jpg', 'Yellow', 249.99, 'Basic knowledge of web development', 'Feature 4', false, 'Active', '98dd3a59-a722-4d2d-822b-2a967cc08df6', NULL, NULL, NULL, NOW(), 'Admin', NOW(), NULL),
    ('7', 'Course 5', 'Description of Course 5', 'https://example.com/photo5.jpg', 'Purple', 349.99, 'Advanced level knowledge of data science', 'Feature 5', true, 'Active', '98dd3a59-a722-4d2d-822b-2a967cc08df6', NULL, NULL, NULL, NOW(), 'Admin', NOW(), NULL),
    ('8', 'Course 6', 'Description of Course 6', 'https://example.com/photo6.jpg', 'Orange', 399.99, 'Expert knowledge of machine learning', 'Feature 6', false, 'Active', '98dd3a59-a722-4d2d-822b-2a967cc08df6', NULL, NULL, NULL, NOW(), 'Admin', NOW(), NULL);


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
VALUES (7, '2', 'Free', 'commisionFee', 18, NOW(), NULL, NULL, NULL);

-- Insert Instructor Pro - Benefit Commission Fee
INSERT INTO public."Subscription" ("Id", "RoleId", "PackageType", "Config", "Value", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES (8, '2', 'Pro', 'commisionFee', 12, NOW(), NULL, NULL, NULL);

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
