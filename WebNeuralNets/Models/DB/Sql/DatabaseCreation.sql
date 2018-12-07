IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181104122558_InitMigration', N'2.1.4-rtm-31024');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'PhoneNumber');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [PhoneNumber];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'PhoneNumberConfirmed');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [PhoneNumberConfirmed];

GO

ALTER TABLE [AspNetUsers] ADD [LanguageCode] int NOT NULL DEFAULT 0;

GO

CREATE TABLE [TranslationValues] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    [LanguageCode] int NOT NULL,
    CONSTRAINT [PK_TranslationValues] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181104122735_AddedTranslation', N'2.1.4-rtm-31024');

GO

DROP INDEX [EmailIndex] ON [AspNetUsers];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Email');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [Email];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'EmailConfirmed');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [EmailConfirmed];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'NormalizedEmail');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [NormalizedEmail];

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'TwoFactorEnabled');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [TwoFactorEnabled];

GO

CREATE TABLE [NeuralNets] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [UserId1] nvarchar(450) NULL,
    CONSTRAINT [PK_NeuralNets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NeuralNets_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_NeuralNets_AspNetUsers_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Layers] (
    [Id] int NOT NULL IDENTITY,
    [NeuralNetId] int NOT NULL,
    [Order] int NOT NULL,
    [Iteration] int NOT NULL,
    CONSTRAINT [PK_Layers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Layers_NeuralNets_NeuralNetId] FOREIGN KEY ([NeuralNetId]) REFERENCES [NeuralNets] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Neurons] (
    [Id] int NOT NULL IDENTITY,
    [LayerId] int NOT NULL,
    [Value] float NOT NULL,
    [Bias] float NOT NULL,
    [Delta] float NOT NULL,
    CONSTRAINT [PK_Neurons] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Neurons_Layers_LayerId] FOREIGN KEY ([LayerId]) REFERENCES [Layers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Dendrites] (
    [Id] int NOT NULL IDENTITY,
    [NextNeuronId] int NOT NULL,
    [PreviousNeuronId] int NOT NULL,
    [Weight] float NOT NULL,
    [Delta] float NOT NULL,
    CONSTRAINT [PK_Dendrites] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Dendrites_Neurons_NextNeuronId] FOREIGN KEY ([NextNeuronId]) REFERENCES [Neurons] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Dendrites_Neurons_PreviousNeuronId] FOREIGN KEY ([PreviousNeuronId]) REFERENCES [Neurons] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Dendrites_NextNeuronId] ON [Dendrites] ([NextNeuronId]);

GO

CREATE INDEX [IX_Dendrites_PreviousNeuronId] ON [Dendrites] ([PreviousNeuronId]);

GO

CREATE INDEX [IX_Layers_NeuralNetId] ON [Layers] ([NeuralNetId]);

GO

CREATE INDEX [IX_NeuralNets_UserId] ON [NeuralNets] ([UserId]);

GO

CREATE INDEX [IX_NeuralNets_UserId1] ON [NeuralNets] ([UserId1]);

GO

CREATE INDEX [IX_Neurons_LayerId] ON [Neurons] ([LayerId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181130181253_NeuralNetStructure', N'2.1.4-rtm-31024');

GO

ALTER TABLE [TranslationValues] DROP CONSTRAINT [PK_TranslationValues];

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TranslationValues]') AND [c].[name] = N'Id');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [TranslationValues] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [TranslationValues] DROP COLUMN [Id];

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TranslationValues]') AND [c].[name] = N'Key');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [TranslationValues] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [TranslationValues] ALTER COLUMN [Key] nvarchar(450) NOT NULL;

GO

ALTER TABLE [TranslationValues] ADD CONSTRAINT [PK_TranslationValues] PRIMARY KEY ([LanguageCode], [Key]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181130192150_TranslationPrimareKey', N'2.1.4-rtm-31024');

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[NeuralNets]') AND [c].[name] = N'Name');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [NeuralNets] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [NeuralNets] ALTER COLUMN [Name] nvarchar(50) NOT NULL;

GO

ALTER TABLE [NeuralNets] ADD [Description] nvarchar(255) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181201123228_NeuralNetTableUpdate', N'2.1.4-rtm-31024');

GO

ALTER TABLE [NeuralNets] DROP CONSTRAINT [FK_NeuralNets_AspNetUsers_UserId1];

GO

DROP INDEX [IX_NeuralNets_UserId1] ON [NeuralNets];

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Neurons]') AND [c].[name] = N'Value');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Neurons] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Neurons] DROP COLUMN [Value];

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[NeuralNets]') AND [c].[name] = N'UserId1');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [NeuralNets] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [NeuralNets] DROP COLUMN [UserId1];

GO

ALTER TABLE [NeuralNets] ADD [TrainingRate] float NOT NULL DEFAULT 0E0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181204212549_FixedStructure', N'2.1.4-rtm-31024');

GO

CREATE TABLE [TrainingData] (
    [Id] int NOT NULL IDENTITY,
    [NeuralNetId] int NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [TrainingSet] nvarchar(max) NULL,
    CONSTRAINT [PK_TrainingData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TrainingData_NeuralNets_NeuralNetId] FOREIGN KEY ([NeuralNetId]) REFERENCES [NeuralNets] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_TrainingData_NeuralNetId] ON [TrainingData] ([NeuralNetId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181206162129_AddedTrainingDataTable', N'2.1.4-rtm-31024');

GO

ALTER TABLE [NeuralNets] ADD [Training] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181206194621_AddedValueToNeuronTable', N'2.1.4-rtm-31024');

GO

ALTER TABLE [NeuralNets] ADD [TrainingIterations] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181206201926_AddedTrainingIterations', N'2.1.4-rtm-31024');

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Neurons]') AND [c].[name] = N'Delta');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Neurons] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Neurons] DROP COLUMN [Delta];

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dendrites]') AND [c].[name] = N'Delta');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Dendrites] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Dendrites] DROP COLUMN [Delta];

GO

INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_FIELDREQUIRED', 'This field is required', 1)

GO

INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_FIELDREQUIRED', 'To pole jest wymagane', 0)

GO

INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_INVALIDTRAININGDATA', 'Invalid training data', 1)

GO

INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_INVALIDTRAININGDATA', 'Błędne dane uczące', 0)

GO

INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('GENERIC_ERROR', 'Oops, something went wrong :(', 1)

GO

INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('GENERIC_ERROR', 'Upss, coś poszło nie tak :(', 0)

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181206210613_NotMappedProps', N'2.1.4-rtm-31024');

GO

ALTER TABLE [NeuralNets] ADD [AverageError] float NOT NULL DEFAULT 0E0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181207162425_AddedError', N'2.1.4-rtm-31024');

GO

