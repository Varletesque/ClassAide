-- ClassAide schema (SQLite 3)
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS courses (
    id   INTEGER PRIMARY KEY AUTOINCREMENT,
    code TEXT NOT NULL COLLATE NOCASE,
    `name` TEXT NOT NULL,
    CONSTRAINT uq_courses_code UNIQUE (code)
);

CREATE TABLE IF NOT EXISTS students (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    first_name  TEXT NOT NULL,
    middle_name TEXT,
    last_name   TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS course_students (
    course_id  INTEGER NOT NULL,
    student_id INTEGER NOT NULL,
    PRIMARY KEY (course_id, student_id),
    CONSTRAINT fk_cs_course  FOREIGN KEY (course_id)  REFERENCES courses(id)  ON DELETE CASCADE,
    CONSTRAINT fk_cs_student FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_course_students_student ON course_students(student_id);

CREATE TABLE IF NOT EXISTS student_groups (
    id        INTEGER PRIMARY KEY AUTOINCREMENT,
    course_id INTEGER NOT NULL,
    `name`      TEXT NOT NULL,
    CONSTRAINT uq_group_course UNIQUE (id, course_id),  -- lets group_members point at (group, course) together
    CONSTRAINT fk_group_course FOREIGN KEY (course_id) REFERENCES courses(id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_groups_course ON student_groups(course_id);

CREATE TABLE IF NOT EXISTS group_members (
    group_id   INTEGER NOT NULL,
    course_id  INTEGER NOT NULL,
    student_id INTEGER NOT NULL,
    PRIMARY KEY (group_id, student_id),
    CONSTRAINT fk_member_group FOREIGN KEY (group_id, course_id)
        REFERENCES student_groups(id, course_id) ON DELETE CASCADE,
    CONSTRAINT fk_member_course_student FOREIGN KEY (course_id, student_id)
        REFERENCES course_students(course_id, student_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_members_student ON group_members(student_id);