CREATE DATABASE affiliate_tracking CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE affiliate_tracking;

drop table if exists affiliate_referrals;
drop table if exists users;
drop table if exists affiliates;

CREATE TABLE affiliates
(
    id         BIGINT      NOT NULL auto_increment,
    code       VARCHAR(64) NOT NULL,
    is_active  TINYINT(1)  NOT NULL DEFAULT 1,
    created_at DATETIME    NOT NULL DEFAULT (utc_timestamp()),
    PRIMARY KEY (id),
    UNIQUE KEY uq_affiliates_code (code)
)
    engine = innodb;

CREATE TABLE users
(
    id            BIGINT       NOT NULL auto_increment,
    email         VARCHAR(255) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at    DATETIME     NOT NULL DEFAULT (utc_timestamp()),
    PRIMARY KEY (id),
    UNIQUE KEY uq_users_email (email)
)
    engine = innodb;

CREATE TABLE affiliate_referrals
(
    id            BIGINT   NOT NULL auto_increment,
    user_id       BIGINT   NOT NULL,
    affiliate_id  BIGINT   NOT NULL,
    clicked_at    DATETIME NULL,
    tracking_id   CHAR(36) NOT NULL,
    attributed_at DATETIME NOT NULL DEFAULT (utc_timestamp()),
    PRIMARY KEY (id),
    UNIQUE KEY uq_referrals_user (user_id),
    KEY ix_referrals_affiliate (affiliate_id),
    CONSTRAINT fk_referrals_user FOREIGN KEY (user_id) REFERENCES users (id) ON
        DELETE CASCADE,
    CONSTRAINT fk_referrals_affiliate FOREIGN KEY (affiliate_id) REFERENCES
        affiliates (id) ON DELETE RESTRICT
)
    engine = innodb; 