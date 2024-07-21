-- Creazione della stored procedure per generare e inserire utenti
CREATE PROCEDURE dbo.CreateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
    @Role NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Salt VARBINARY(128);
    DECLARE @Hash VARBINARY(64);

    -- Genera il salt
    SET @Salt = CRYPT_GEN_RANDOM(128);

    -- Genera l'hash della password
    SET @Hash = HASHBYTES('SHA2_512', CONVERT(VARBINARY, @Password) + @Salt);

    -- Inserisci l'utente nella tabella Utenti
    INSERT INTO [dbo].[Utenti] (Username, PasswordHash, PasswordSalt, Role)
    VALUES (@Username, @Hash, @Salt, @Role);
END;
GO

-- Creazione dell'utente 'user' con password '1234'
EXEC dbo.CreateUser 'user', '1234', 'User';

-- Creazione dell'utente 'admin' con password '1234'
EXEC dbo.CreateUser 'admin', '1234', 'Admin';
GO

-- Eliminazione della stored procedure (se desiderato)
DROP PROCEDURE dbo.CreateUser;
GO
