INSERT INTO affiliates (code, is_active)
VALUES ('HeyCola', 1),
       ('Sevan2026', 1),
       ('Sevan2025', 0);

-- Get affiliate by code
CALL sp_affiliate_get_by_code('HeyCola');
CALL sp_affiliate_get_by_code('Sevan2026');
CALL sp_affiliate_get_by_code('Sevan2025');
CALL sp_affiliate_get_by_code('UnknownCode');

-- Register with active affiliate (status = 0, attributed = 1)
CALL sp_user_register('erik@test.com', 'hashed_pwd_123', 1, '550e8400-e29b-41d4-a716-446655440000', utc_timestamp());

-- Register with same email (status = 1, duplicate email)
CALL sp_user_register('erik@test.com', 'hashed_pwd_123', 1, '550e8400-e29b-41d4-a716-446655440000', utc_timestamp());

-- Register direct without affiliate (status = 0, attributed = 0)
CALL sp_user_register('direct@test.com', 'hashed_pwd_123', NULL, NULL, NULL);

-- Register with non-existing affiliate (status = 0, attributed = 0)
CALL sp_user_register('koko@test.com', 'hashed_pwd_123', 999999, '33333333-3333-3333-3333-333333333333', utc_timestamp());

SELECT * FROM users;
SELECT * FROM affiliate_referrals;