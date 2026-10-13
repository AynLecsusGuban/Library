-- schema.sql : the Library Lending System database
-- Run this first, once. It removes any existing lending schema and rebuilds it empty.
 
DROP SCHEMA IF EXISTS lending CASCADE;
CREATE SCHEMA lending;
 
CREATE TABLE lending.member (
  member_id    BIGINT       GENERATED ALWAYS AS IDENTITY,
  full_name    VARCHAR(80)  NOT NULL,
  email        VARCHAR(120),
  join_date    DATE         NOT NULL DEFAULT CURRENT_DATE,
  member_type  VARCHAR(10)  NOT NULL DEFAULT 'Regular',
  CONSTRAINT pk_member       PRIMARY KEY (member_id),
  CONSTRAINT uq_member_email UNIQUE (email),
  CONSTRAINT chk_member_type CHECK (member_type IN ('Regular','Student','Faculty'))
);
 
CREATE TABLE lending.book (
  book_id   BIGINT        GENERATED ALWAYS AS IDENTITY,
  title     VARCHAR(150)  NOT NULL,
  category  VARCHAR(40),
  price     NUMERIC(7,2),
  CONSTRAINT pk_book            PRIMARY KEY (book_id),
  CONSTRAINT chk_price_positive CHECK (price IS NULL OR price > 0)
);
 
CREATE TABLE lending.loan (
  loan_id       BIGINT   GENERATED ALWAYS AS IDENTITY,
  member_id     BIGINT   NOT NULL,
  book_id       BIGINT   NOT NULL,
  loaned_on     DATE     NOT NULL DEFAULT CURRENT_DATE,
  returned_on   DATE,
  days_allowed  INTEGER  NOT NULL DEFAULT 7,
  CONSTRAINT pk_loan        PRIMARY KEY (loan_id),
  CONSTRAINT fk_loan_member FOREIGN KEY (member_id) REFERENCES lending.member (member_id) ON DELETE RESTRICT,
  CONSTRAINT fk_loan_book   FOREIGN KEY (book_id)   REFERENCES lending.book (book_id)     ON DELETE CASCADE,
  CONSTRAINT chk_days_allowed      CHECK (days_allowed BETWEEN 1 AND 30),
  CONSTRAINT chk_return_after_loan CHECK (returned_on IS NULL OR returned_on >= loaned_on)
);