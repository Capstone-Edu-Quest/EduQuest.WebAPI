CREATE OR REPLACE FUNCTION trg_update_stage_total_time()
RETURNS TRIGGER AS $$
DECLARE
    v_stage_id TEXT;
BEGIN
    -- Xác định StageId bị ảnh hưởng
    IF TG_OP = 'INSERT' THEN
        v_stage_id := CAST(NEW."StageId" AS TEXT);
    ELSIF TG_OP = 'UPDATE' THEN
        v_stage_id := CAST(NEW."StageId" AS TEXT);
    ELSIF TG_OP = 'DELETE' THEN
        v_stage_id := CAST(OLD."StageId" AS TEXT);
    END IF;
    
    -- Nếu là DELETE, chỉ cần cập nhật giá trị TotalTime trong Stage
    IF TG_OP = 'DELETE' THEN
        UPDATE public."Stage"
        SET "TotalTime" = COALESCE((
            SELECT SUM(lm."Duration")
            FROM public."LearningMaterial" lm
            WHERE CAST(lm."StageId" AS TEXT) = v_stage_id
        ), 0)
        WHERE CAST("Id" AS TEXT) = v_stage_id;
        RETURN NULL;
    END IF;
    
    -- Cập nhật TotalTime của Stage
    UPDATE public."Stage"
    SET "TotalTime" = COALESCE((
        SELECT SUM(lm."Duration")
        FROM public."LearningMaterial" lm
        WHERE CAST(lm."StageId" AS TEXT) = v_stage_id
    ), 0)
    WHERE CAST("Id" AS TEXT) = v_stage_id;
    
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_stage_total_time
AFTER INSERT OR UPDATE OR DELETE
ON public."LearningMaterial"
FOR EACH ROW
EXECUTE FUNCTION trg_update_stage_total_time();
