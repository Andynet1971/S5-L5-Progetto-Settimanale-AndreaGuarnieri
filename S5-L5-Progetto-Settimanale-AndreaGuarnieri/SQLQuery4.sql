-- Aggiungi utente Admin
DECLARE @PasswordSalt VARBINARY(64)
DECLARE @PasswordHash VARBINARY(64)

-- Genera un salt casuale
SET @PasswordSalt = CAST(CRYPT_GEN_RANDOM(64) AS VARBINARY(64))

-- Calcola l'hash della password combinata con il salt
SET @PasswordHash = HASHBYTES('SHA2_512', CONVERT(NVARCHAR(MAX), '1234') + CONVERT(NVARCHAR(MAX), @PasswordSalt))

INSERT INTO Utenti (Username, PasswordHash, PasswordSalt, Role)
VALUES ('admin', @PasswordHash, @PasswordSalt, 'Admin')

-- Aggiungi utente User
DECLARE @UserPasswordSalt VARBINARY(64)
DECLARE @UserPasswordHash VARBINARY(64)

-- Genera un salt casuale
SET @UserPasswordSalt = CAST(CRYPT_GEN_RANDOM(64) AS VARBINARY(64))

-- Calcola l'hash della password combinata con il salt
SET @UserPasswordHash = HASHBYTES('SHA2_512', CONVERT(NVARCHAR(MAX), '1234') + CONVERT(NVARCHAR(MAX), @UserPasswordSalt))

INSERT INTO Utenti (Username, PasswordHash, PasswordSalt, Role)
VALUES ('user', @UserPasswordHash, @UserPasswordSalt, 'User')
