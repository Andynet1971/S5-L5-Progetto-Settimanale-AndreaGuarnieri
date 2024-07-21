CREATE TABLE [dbo].[Utenti] (
    [Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR (50) NOT NULL UNIQUE,
    [PasswordHash] VARBINARY (64) NOT NULL,
    [PasswordSalt] VARBINARY (128) NOT NULL,
    [Role] NVARCHAR (10) NOT NULL
);
