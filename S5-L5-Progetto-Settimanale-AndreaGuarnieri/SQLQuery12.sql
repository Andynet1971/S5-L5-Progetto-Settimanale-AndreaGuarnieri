-- Creazione della tabella Utenti
CREATE TABLE [dbo].[Utenti] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL,
    [PasswordHash] VARBINARY(MAX) NOT NULL,
    [Role] NVARCHAR(10) NOT NULL
);

-- Rimuovi i vecchi utenti
DELETE FROM Utenti;

-- Aggiungi utente admin
INSERT INTO Utenti (Username, PasswordHash, Role)
VALUES ('admin', CONVERT(VARBINARY(MAX), '2a2f6c1ef9d946c1a32dd0a76b1b9467f07e85b67ddae2f941e2c9915d6a6493'), 'Admin');

-- Aggiungi utente user
INSERT INTO Utenti (Username, PasswordHash, Role)
VALUES ('user', CONVERT(VARBINARY(MAX), '2a2f6c1ef9d946c1a32dd0a76b1b9467f07e85b67ddae2f941e2c9915d6a6493'), 'User');
