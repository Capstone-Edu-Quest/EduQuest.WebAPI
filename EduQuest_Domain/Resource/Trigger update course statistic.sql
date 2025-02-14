CREATE OR REPLACE FUNCTION trg_update_course_statistic()
RETURNS TRIGGER AS $$
DECLARE
    v_course_id TEXT;
BEGIN
    -- Xác định CourseId bị ảnh hưởng
    IF TG_OP = 'INSERT' THEN
        v_course_id := CAST(NEW."Id" AS TEXT);
    ELSIF TG_OP = 'UPDATE' THEN
        v_course_id := CAST(NEW."Id" AS TEXT);
    ELSIF TG_OP = 'DELETE' THEN
        v_course_id := CAST(OLD."Id" AS TEXT);
    END IF;
    
    -- Nếu là DELETE, chỉ cần xóa dữ liệu cũ
    IF TG_OP = 'DELETE' THEN
        DELETE FROM public."CourseStatistic" WHERE "CourseId" = v_course_id;
        RETURN NULL;
    END IF;
    
    -- Xóa dữ liệu cũ trước khi cập nhật
    DELETE FROM public."CourseStatistic" WHERE "CourseId" = v_course_id;
    
    -- Tính toán lại thống kê và chèn dữ liệu mới
    INSERT INTO public."CourseStatistic" ("Id", "CourseId", "TotalLesson", "TotalTime", "TotalLearner", "Rating", "TotalReview", "CreatedAt", "UpdatedBy", "UpdatedAt", "DeletedAt")
    SELECT 
        CAST(gen_random_uuid() AS TEXT) AS "Id",
        v_course_id AS "CourseId",
        COALESCE(lm."TotalLesson", 0) AS "TotalLesson",
        COALESCE(st."TotalTime", 0) AS "TotalTime",
        COALESCE(lr."TotalLearner", 0) AS "TotalLearner",
        COALESCE(fb."Rating", 0) AS "Rating",
        COALESCE(fb."TotalReview", 0) AS "TotalReview",
        NOW() AS "CreatedAt",
        NULL AS "UpdatedBy",
        NOW() AS "UpdatedAt",
        NULL AS "DeletedAt"
    FROM (SELECT v_course_id AS "CourseId") AS c
    LEFT JOIN LATERAL 
        (SELECT COUNT(lm."StageId") AS "TotalLesson"
         FROM public."Stage" s
         LEFT JOIN public."LearningMaterial" lm ON lm."StageId" = s."Id"
         WHERE s."CourseId" = c."CourseId"
         GROUP BY s."CourseId") lm ON TRUE
    LEFT JOIN LATERAL
        (SELECT SUM(s."TotalTime") AS "TotalTime"
         FROM public."Stage" s
         WHERE s."CourseId" = c."CourseId"
         GROUP BY s."CourseId") st ON TRUE
    LEFT JOIN LATERAL
        (SELECT COUNT(DISTINCT l."UserId") AS "TotalLearner"
         FROM public."Learner" l
         WHERE l."CourseId" = c."CourseId"
         GROUP BY l."CourseId") lr ON TRUE
    LEFT JOIN LATERAL
        (SELECT AVG(f."Rating") AS "Rating", COUNT(f."Id") AS "TotalReview"
         FROM public."Feedback" f
         WHERE f."CourseId" = c."CourseId"
         GROUP BY f."CourseId") fb ON TRUE;
    
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_course_statistic
AFTER INSERT OR UPDATE OR DELETE
ON public."Course"
FOR EACH ROW
EXECUTE FUNCTION trg_update_course_statistic();
