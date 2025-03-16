

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
    ('4', 'Assignment', 1.0, 'Configuration for assignment', NOW(), NOW()),
	('5', 'CourseFee', 0.18, 'Configuration for course', NOW(), NOW());


INSERT INTO "Subscription" ("Id", "Name", "Description", "DurationDays", "Price", "IsFree", "CreatedAt", "UpdatedAt", "UpdatedBy", "DeletedAt")
VALUES
    ('1', 'Pro Account Month', 'des1', 30, 10, TRUE, CURRENT_TIMESTAMP, NULL, NULL, NULL),
    ('2', 'Pro Account Year', 'des2', 365, 80, FALSE, CURRENT_TIMESTAMP, NULL, NULL, NULL);
   

INSERT INTO "User" ("Id", "Username", "AvatarUrl", "Email", "Phone", "Headline", "Status", "Description", "RoleId","SubscriptionId", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
VALUES
    (1, 'user1', 'avatar1.png', 'user1@example.com', '1234567890', 'Headline 1', 'Active', 'Description 1', 1, 1, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, NULL),
    (2, 'user2', 'avatar2.png', 'user2@example.com', '1234567891', 'Headline 2', 'Active', 'Description 2', 2, 2, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, NULL),
    (3, 'user3', 'avatar3.png', 'user3@example.com', '1234567892', 'Headline 3', 'Active', 'Description 3', 3, 3, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, NULL),
    (4, 'user4', 'avatar4.png', 'user4@example.com', '1234567893', 'Headline 4', 'Active', 'Description 4', 4, 4, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, NULL),
    (5, 'user5', 'avatar5.png', 'user5@example.com', '1234567894', 'Headline 5', 'Active', 'Description 5', 5, 3, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, NULL),
    (6, 'user6', 'avatar6.png', 'user6@example.com', '1234567895', 'Headline 6', 'Active', 'Description 6', 6, 2, CURRENT_TIMESTAMP, NULL, CURRENT_TIMESTAMP, NULL);





INSERT INTO "Course" ("Id", "Title", "Description", "PhotoUrl", "Color", "Price", "Requirement", "Feature", "IsRequired", "CreatedBy",
                      "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt", "Status")
VALUES
    (1, 'Course Title 1', 'Description for Course 1', 'photo1.png', '#FF5733', 99.99, 'Requirement for Course 1', 'Feature for Course 1', TRUE, 2,
     CURRENT_TIMESTAMP, NULL, null, NULL, 'Published'),

    (2, 'Course Title 2', 'Description for Course 2', 'photo2.png', '#33FF57', 149.99, 'Requirement for Course 2', 'Feature for Course 2', FALSE, 2,
     CURRENT_TIMESTAMP, NULL, null, NULL, 'Published');
