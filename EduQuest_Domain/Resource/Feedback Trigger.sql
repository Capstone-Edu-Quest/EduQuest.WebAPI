CREATE OR REPLACE FUNCTION update_course_rating()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE "CourseStatistic"
    SET "Rating" = ROUND(
        (SELECT AVG("Rating") FROM "Feedback" WHERE "CourseId" = NEW."CourseId"),
        2 -- Giữ 2 chữ số thập phân
    )
    WHERE "CourseId" = NEW."CourseId";

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER trg_update_course_rating
AFTER INSERT OR UPDATE OR DELETE ON "Feedback"
FOR EACH ROW
EXECUTE FUNCTION update_course_rating();

INSERT INTO "Feedback" ("UserId", "CourseId", "Rating", "Comment", "CreatedAt")
VALUES ('2dc2ce78-b931-4d7f-aa88-6237a4985098', 'ed2371fa-4df6-4c92-8a9c-d69546063b4a', 4, 'Great course!', NOW());

-- Kiểm tra xem rating đã cập nhật chưa
SELECT * FROM "CourseStatistic" WHERE "CourseId" = 'ed2371fa-4df6-4c92-8a9c-d69546063b4a';

