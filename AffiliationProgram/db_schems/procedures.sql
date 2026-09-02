DROP PROCEDURE IF EXISTS sp_affiliate_get_by_code;
DROP PROCEDURE IF EXISTS sp_user_register;

CREATE PROCEDURE sp_affiliate_get_by_code(
    IN _code VARCHAR(64)
)
BEGIN
    SELECT id, code, is_active
    FROM affiliates
    WHERE code = _code;
END;

CREATE PROCEDURE sp_user_register(
    IN _email VARCHAR(255),
    IN _password_hash VARCHAR(255),
    IN _affiliate_id BIGINT,
    IN _tracking_id CHAR(36),
    IN _clicked_at DATETIME
)
BEGIN
    DECLARE _new_user_id BIGINT;
    DECLARE _attributed TINYINT UNSIGNED DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SELECT 2 AS status, NULL AS user_id, 0 AS attributed;
    END;

    IF EXISTS (SELECT 1 FROM users WHERE email = _email) THEN
        SELECT 1 AS status, NULL AS user_id, 0 AS attributed;
    ELSE
        START TRANSACTION;

        INSERT INTO users (email, password_hash)
        VALUES (_email, _password_hash);

        SET _new_user_id = LAST_INSERT_ID();

        IF _affiliate_id IS NOT NULL AND EXISTS (
            SELECT 1 FROM affiliates WHERE id = _affiliate_id AND is_active = 1
        ) THEN
            INSERT INTO affiliate_referrals (user_id, affiliate_id, tracking_id, clicked_at)
            VALUES (_new_user_id, _affiliate_id, _tracking_id, _clicked_at);

            SET _attributed = 1;
        END IF;

        COMMIT;

        SELECT 0 AS status, _new_user_id AS user_id, _attributed AS attributed;
    END IF;
END;