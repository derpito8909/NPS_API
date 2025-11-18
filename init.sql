
IF DB_ID('NpsDb') IS NULL
BEGIN
    CREATE DATABASE NpsDb;
END;
GO

USE NpsDb;
GO


IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id                  INT             IDENTITY(1,1) NOT NULL,
        Username            NVARCHAR(100)   NOT NULL,
        PasswordHash        NVARCHAR(500)   NOT NULL,
        Role                NVARCHAR(20)    NOT NULL,  -- 'Admin' o 'Voter'
        IsLocked            BIT             NOT NULL    CONSTRAINT DF_Users_IsLocked DEFAULT(0),
        FailedLoginAttempts INT             NOT NULL    CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT(0),        
        LastLoginAt      DATETIME2(0)    NULL,
        CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id)
    );

    CREATE UNIQUE INDEX UX_Users_Username
        ON dbo.Users (Username);

    ALTER TABLE dbo.Users WITH CHECK
    ADD CONSTRAINT CK_Users_Role
        CHECK (Role IN ('Admin', 'Voter'));
END;
GO


IF OBJECT_ID('dbo.RefreshTokens', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RefreshTokens
    (
        Id          INT             IDENTITY(1,1) NOT NULL,
        UserId      INT             NOT NULL,
        Token       NVARCHAR(512)   NOT NULL,
        ExpiresAt   DATETIME2(0)    NOT NULL,  -- UTC
        IsRevoked   BIT             NOT NULL    CONSTRAINT DF_RefreshTokens_IsRevoked DEFAULT(0),
        CONSTRAINT PK_RefreshTokens PRIMARY KEY CLUSTERED (Id)
    );

    ALTER TABLE dbo.RefreshTokens WITH CHECK
    ADD CONSTRAINT FK_RefreshTokens_Users
        FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
        ON DELETE CASCADE;

    CREATE UNIQUE INDEX UX_RefreshTokens_Token
        ON dbo.RefreshTokens (Token);

    CREATE INDEX IX_RefreshTokens_UserId
        ON dbo.RefreshTokens (UserId);
END;
GO

IF OBJECT_ID('dbo.NpsQuestions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.NpsQuestions
    (
        Id              INT             IDENTITY(1,1) NOT NULL,
        QuestionText    NVARCHAR(500)   NOT NULL,
        IsActive        BIT             NOT NULL    CONSTRAINT DF_NpsQuestions_IsActive DEFAULT(0),
        CONSTRAINT PK_NpsQuestions PRIMARY KEY CLUSTERED (Id)
    );
END;
GO


IF OBJECT_ID('dbo.NpsVotes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.NpsVotes
    (
        Id              INT             IDENTITY(1,1) NOT NULL,
        UserId          INT             NOT NULL,
        QuestionId      INT             NOT NULL,
        Score           INT             NOT NULL,
        CreatedAt      DATETIME2(0)    NOT NULL    CONSTRAINT DF_NpsVotes_VotedAtUtc DEFAULT(SYSUTCDATETIME()),
        CONSTRAINT PK_NpsVotes PRIMARY KEY CLUSTERED (Id)
    );

    ALTER TABLE dbo.NpsVotes WITH CHECK
    ADD CONSTRAINT FK_NpsVotes_Users
        FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);

    ALTER TABLE dbo.NpsVotes WITH CHECK
    ADD CONSTRAINT FK_NpsVotes_NpsQuestions
        FOREIGN KEY (QuestionId) REFERENCES dbo.NpsQuestions(Id);

    CREATE UNIQUE INDEX UX_NpsVotes_User_Question
        ON dbo.NpsVotes (UserId, QuestionId);

    ALTER TABLE dbo.NpsVotes WITH CHECK
    ADD CONSTRAINT CK_NpsVotes_Score
        CHECK (Score BETWEEN 0 AND 10);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.NpsQuestions WHERE IsActive = 1)
BEGIN
    INSERT INTO dbo.NpsQuestions (QuestionText, IsActive)
    VALUES ('¿Qué tan probable es que recomiende nuestro servicio a un amigo o familiar?', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'voter1')
BEGIN
    INSERT INTO dbo.Users (Username, PasswordHash, Role, IsLocked, FailedLoginAttempts)
    VALUES
    (
        'votante1',
        '$2a$11$bgZoClCBYKvDkhE5IHmdJO6pG4L9G0pp27RcbR.Ha2qNu/J3fEsNi',
        'Voter',
        0,
        0
    );
END;
GO
