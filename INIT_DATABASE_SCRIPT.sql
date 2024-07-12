-- Check if the database exists, if not, create it
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TestAppApi')
BEGIN
    CREATE DATABASE TestAppApi;
    PRINT 'TestAppApi database created successfully.';
END
ELSE
BEGIN
    PRINT 'TestAppApi database already exists.';
END

-- Use the TestAppApi database
USE TestAppApi;

-- Check if the Users table exists, if not, create it
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    -- Create the Users table
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL
    );

    PRINT 'Users table created successfully.';
END
ELSE
-- Check if the Subjects table exists, if not, create it
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Subjects')
BEGIN
    -- Create the Subjects table
    CREATE TABLE Subjects (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL
    );

    PRINT 'Subjects table created successfully.';
END
ELSE
BEGIN
    PRINT 'Subjects table already exists.';
END

-- Check if the StudyGroups table exists, if not, create it
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudyGroups')
BEGIN
    -- Create the StudyGroups table with foreign key constraint
    CREATE TABLE StudyGroups (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SubjectId INT,
        Name NVARCHAR(30) CHECK (LEN(Name) BETWEEN 5 AND 30) NOT NULL,
        CreationDate DATETIME,
        FOREIGN KEY (SubjectId) REFERENCES Subjects(Id)
    );

    PRINT 'StudyGroups table created successfully.';
END
ELSE
BEGIN
    PRINT 'Users table already exists.';
END



-- Insert data into Subjects table
INSERT INTO Subjects (Name)
VALUES ('Math'), ('Chemistry'), ('Physics');

PRINT 'Data inserted into Subjects table successfully.';

-- Insert data into Users table with predefined names
INSERT INTO Users (Name)
VALUES ('John Smith');
INSERT INTO Users (Name)
VALUES ('Mary Johnson');
INSERT INTO Users (Name)
VALUES ('David Williams');
INSERT INTO Users (Name)
VALUES ('Lisa Jones');
INSERT INTO Users (Name)
VALUES ('Michael Brown');
INSERT INTO Users (Name)
VALUES ('Emma Davis');
INSERT INTO Users (Name)
VALUES ('James Miller');
INSERT INTO Users (Name)
VALUES ('Olivia Wilson');
INSERT INTO Users (Name)
VALUES ('Liam Moore');
INSERT INTO Users (Name)
VALUES ('Sophia Taylor');


-- Check if the UserStudyGroups table exists, if not, create it
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserStudyGroups')
BEGIN
    -- Create the UserStudyGroups table with foreign key constraints
    CREATE TABLE UserStudyGroups (
        UserId INT,
        StudyGroupId INT,
        FOREIGN KEY (UserId) REFERENCES Users(Id),
        FOREIGN KEY (StudyGroupId) REFERENCES StudyGroups(Id)
    );

    PRINT 'UserStudyGroups table created successfully.';
END
ELSE
BEGIN
    PRINT 'UserStudyGroups table already exists.';
END


