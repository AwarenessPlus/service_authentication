IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE TABLE [Authentication] (
        [AuthenticationID] int NOT NULL IDENTITY,
        [UserName] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        CONSTRAINT [PK_Authentication] PRIMARY KEY ([AuthenticationID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE TABLE [User] (
        [UserID] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NULL,
        [SecondName] nvarchar(max) NULL,
        [Surname] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Age] int NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([UserID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE TABLE [Medic] (
        [MedicID] int NOT NULL IDENTITY,
        [Rotation] nvarchar(max) NULL,
        [Semester] int NOT NULL,
        [UserID] int NOT NULL,
        [AuthenticationID] int NOT NULL,
        CONSTRAINT [PK_Medic] PRIMARY KEY ([MedicID]),
        CONSTRAINT [FK_Medic_Authentication_AuthenticationID] FOREIGN KEY ([AuthenticationID]) REFERENCES [Authentication] ([AuthenticationID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Medic_User_UserID] FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE TABLE [Pacient] (
        [PacientID] int NOT NULL IDENTITY,
        [Bloodgroup] int NULL,
        [Rh] int NULL,
        [Sex] int NULL,
        [UserID] int NOT NULL,
        CONSTRAINT [PK_Pacient] PRIMARY KEY ([PacientID]),
        CONSTRAINT [FK_Pacient_User_UserID] FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE TABLE [Procedure] (
        [ProcedureID] int NOT NULL IDENTITY,
        [ProcedureName] nvarchar(max) NULL,
        [PatientStatus] nvarchar(max) NULL,
        [Asa] int NOT NULL,
        [MedicID] int NOT NULL,
        [_pacientID] int NOT NULL,
        [VideoRecord] varbinary(max) NULL,
        [PacientID] int NOT NULL,
        CONSTRAINT [PK_Procedure] PRIMARY KEY ([ProcedureID]),
        CONSTRAINT [FK_Procedure_Pacient__pacientID] FOREIGN KEY ([_pacientID]) REFERENCES [Pacient] ([PacientID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE INDEX [IX_Medic_AuthenticationID] ON [Medic] ([AuthenticationID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE INDEX [IX_Medic_UserID] ON [Medic] ([UserID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE INDEX [IX_Pacient_UserID] ON [Pacient] ([UserID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    CREATE INDEX [IX_Procedure__pacientID] ON [Procedure] ([_pacientID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211011153535_DatabaseMigrations')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211011153535_DatabaseMigrations', N'5.0.10');
END;
GO

COMMIT;
GO

