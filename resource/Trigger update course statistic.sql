CREATE TRIGGER trg_UpdateCourseStatistic
ON Course
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table to store affected CourseIds
    DECLARE @AffectedCourses TABLE (CourseId UNIQUEIDENTIFIER);

    -- Capture affected CourseIds from inserted and deleted tables
    INSERT INTO @AffectedCourses (CourseId)
    SELECT DISTINCT Id FROM inserted
    UNION 
    SELECT DISTINCT Id FROM deleted;

    -- Delete outdated statistics for affected courses
    DELETE FROM CourseStatistic
    WHERE CourseId IN (SELECT CourseId FROM @AffectedCourses);

    -- Recalculate and insert updated statistics
    INSERT INTO CourseStatistic (Id, CourseId, TotalLesson, TotalTime, TotalLearner, Rating, TotalReview, CreatedAt, UpdatedAt)
    SELECT 
        CONVERT(NVARCHAR(36), NEWID()) AS Id,  -- Store GUID as string
        ac.CourseId,
        COALESCE(lm.TotalLesson, 0) AS TotalLesson,
        COALESCE(st.TotalTime, 0) AS TotalTime,
        COALESCE(lr.TotalLearner, 0) AS TotalLearner,
        COALESCE(fb.Rating, 0) AS Rating,
        COALESCE(fb.TotalReview, 0) AS TotalReview,
        GETDATE() AS CreatedAt,
        GETDATE() AS UpdatedAt
    FROM @AffectedCourses ac
    LEFT JOIN 
        (SELECT s.CourseId, COUNT(lm.StageId) AS TotalLesson
         FROM Stage s
         LEFT JOIN LearningMaterial lm ON lm.StageId = s.Id
         GROUP BY s.CourseId) lm ON ac.CourseId = lm.CourseId
    LEFT JOIN 
        (SELECT s.CourseId, SUM(s.TotalTime) AS TotalTime
         FROM Stage s
         GROUP BY s.CourseId) st ON ac.CourseId = st.CourseId
    LEFT JOIN 
        (SELECT l.CourseId, COUNT(DISTINCT l.UserId) AS TotalLearner
         FROM Learner l
         GROUP BY l.CourseId) lr ON ac.CourseId = lr.CourseId
    LEFT JOIN 
        (SELECT f.CourseId, AVG(f.Rating) AS Rating, COUNT(f.Id) AS TotalReview
         FROM Feedback f
         GROUP BY f.CourseId) fb ON ac.CourseId = fb.CourseId;
END;

CREATE TRIGGER trg_UpdateCourseStatistic_Feedback
ON Feedback
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CourseStatistic
    SET UpdatedAt = GETDATE()
    WHERE CourseId IN (SELECT DISTINCT CourseId FROM inserted UNION SELECT DISTINCT CourseId FROM deleted);
END;

CREATE TRIGGER trg_UpdateCourseStatistic_Learner
ON Learner
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CourseStatistic
    SET UpdatedAt = GETDATE()
    WHERE CourseId IN (SELECT DISTINCT CourseId FROM inserted UNION SELECT DISTINCT CourseId FROM deleted);
END;


CREATE TRIGGER trg_UpdateCourseStatistic_Learner
ON Learner
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CourseStatistic
    SET UpdatedAt = GETDATE()
    WHERE CourseId IN (SELECT DISTINCT CourseId FROM inserted UNION SELECT DISTINCT CourseId FROM deleted);
END;

CREATE TRIGGER trg_UpdateCourseStatistic_Stage
ON Stage
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CourseStatistic
    SET UpdatedAt = GETDATE()
    WHERE CourseId IN (SELECT DISTINCT CourseId FROM inserted UNION SELECT DISTINCT CourseId FROM deleted);
END;

CREATE TRIGGER trg_UpdateCourseStatistic_LearningMaterial
ON LearningMaterial
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CourseStatistic
    SET UpdatedAt = GETDATE()
    WHERE CourseId IN (
        SELECT DISTINCT s.CourseId
        FROM inserted lm
        JOIN Stage s ON lm.StageId = s.Id
        UNION
        SELECT DISTINCT s.CourseId
        FROM deleted lm
        JOIN Stage s ON lm.StageId = s.Id
    );
END;




