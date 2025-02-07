CREATE TRIGGER trg_UpdateCourseStatistic
ON Feedback, Learner, Stage, LearningMaterial
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Recalculate Course Statistics
    UPDATE cs
    SET 
        cs.Rating = COALESCE(fb.AvgRating, 0),
        cs.TotalReview = COALESCE(fb.TotalReview, 0),
        cs.TotalLearner = COALESCE(lr.TotalLearner, 0),
        cs.TotalTime = COALESCE(st.TotalTime, 0),
        cs.TotalLesson = COALESCE(lm.TotalLesson, 0)
    FROM CourseStatistic cs
    LEFT JOIN 
        (SELECT f.CourseId, 
                AVG(f.Rating) AS AvgRating, 
                COUNT(f.Id) AS TotalReview
         FROM Feedback f
         GROUP BY f.CourseId) fb ON cs.CourseId = fb.CourseId
    LEFT JOIN 
        (SELECT l.CourseId, COUNT(DISTINCT l.UserId) AS TotalLearner
         FROM Learner l
         GROUP BY l.CourseId) lr ON cs.CourseId = lr.CourseId
    LEFT JOIN 
        (SELECT s.CourseId, SUM(s.TotalTime) AS TotalTime
         FROM Stage s
         GROUP BY s.CourseId) st ON cs.CourseId = st.CourseId
    LEFT JOIN 
        (SELECT s.CourseId, COUNT(lm.StageId) AS TotalLesson
         FROM LearningMaterial lm
         JOIN Stage s ON lm.StageId = s.StageId
         GROUP BY s.CourseId) lm ON cs.CourseId = lm.CourseId;
END;
