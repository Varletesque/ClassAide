-- ClassAide schema (MySQL 8+, InnoDB)

CREATE DATABASE IF NOT EXISTS classaide
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE classaide;

CREATE TABLE IF NOT EXISTS courses (
    id   INT UNSIGNED NOT NULL AUTO_INCREMENT,
    code VARCHAR(20)  NOT NULL,
    name VARCHAR(150) NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uq_courses_code (code)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS students (
    id         INT UNSIGNED NOT NULL AUTO_INCREMENT,
    first_name VARCHAR(100) NOT NULL,
    last_name  VARCHAR(100) NOT NULL,
    PRIMARY KEY (id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS course_students (
    course_id  INT UNSIGNED NOT NULL,
    student_id INT UNSIGNED NOT NULL,
    PRIMARY KEY (course_id, student_id),
    KEY idx_course_students_student (student_id),
    CONSTRAINT fk_cs_course  FOREIGN KEY (course_id)  REFERENCES courses(id)  ON DELETE CASCADE,
    CONSTRAINT fk_cs_student FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS student_groups (
    id        INT UNSIGNED NOT NULL AUTO_INCREMENT,
    course_id INT UNSIGNED NOT NULL,
    name      VARCHAR(100) NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY uq_group_course (id, course_id),  -- lets group_members point at (group, course) together
    KEY idx_groups_course (course_id),
    CONSTRAINT fk_group_course FOREIGN KEY (course_id) REFERENCES courses(id) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS group_members (
    group_id   INT UNSIGNED NOT NULL,
    course_id  INT UNSIGNED NOT NULL,
    student_id INT UNSIGNED NOT NULL,
    PRIMARY KEY (group_id, student_id),
    KEY idx_members_student (student_id),
    CONSTRAINT fk_member_group      FOREIGN KEY (group_id, course_id)
        REFERENCES student_groups(id, course_id) ON DELETE CASCADE,
    CONSTRAINT fk_member_course_student FOREIGN KEY (course_id, student_id)
        REFERENCES course_students(course_id, student_id) ON DELETE CASCADE
) ENGINE=InnoDB;
