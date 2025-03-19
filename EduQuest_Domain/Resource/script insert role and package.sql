

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

INSERT INTO public."Subscription"(
    "Id", "Package", "Type", "MonthlyPrice", "YearlyPrice", 
    "Value", "BenefitsJson", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt"
)
VALUES
    (1, 'Instructor Pro', 'Pro', 10, 95, 18, 
     '{"Commision Fee": "$18", "Marketing Email Per Month": "$3"}'::json, NOW(), 'Admin', NOW(), NULL),
    (2, 'Learner Pro', 'Pro', 5, 50, 12, 
     '{"Coupon Per Month": "$3", "Coupon Discount Upto": "$90", "Extra Gold and Exp": "$10", "Trial Course Percentage": "$15", "Course Trial Per Month": "$5"}'::json, 
     NOW(), 'Admin', NOW(), NULL),
    (3, 'Instructor Free', 'Free', 0, 0, 0, 
     '{"Commision Fee": "$12"}'::json, NOW(), 'Admin', NOW(), NULL);
