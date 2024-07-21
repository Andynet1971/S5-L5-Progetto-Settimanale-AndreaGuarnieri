Ho strutturato l'applicazione seguendo le linee guida della traccia ma ho aggiunto funzionalità aggiuntive non richieste per verificare la mia capacità di implementarle correttamente.
All'avvio dell'applicazione si apre una pagina di Login che poi dara accesso a tutte le funzionalità. Il login è strutturato con autenticazione ed autorizzazioni. Ho creato una tabella ulteriore nel database con utente, password e ruolo. Ho impostato 2 ruoli: user e admin.
Ho inserito sotto il login form una check box (Remember me) che consente di impostare il tempo di validità del cookie che ho impostato a 15 minuti.
Se si accede come user nella navbar sarà visualizzato solo la voce visualizzazioni che porterà ad una pagina che consente, tramite 4 link, di accedere alle 4 viste come da traccia del compito. Se invece si accede come admin nella navbar sarà visualizzato, oltre alla voce visualizzazioni, altre 2 voci: Elenco Violazioni e Compilazione Verbale. Nella vista elenco violazioni verranno stampate a video tutte le possibili violazioni in archivio e per mezzo di 2 bottoni sarà possibile sia la modifica della visualizzazione (che verrà fatta in una pagina dettaglio) sia la cancellazione della violazione. La cancellazione della violazione controllerà tutto il database e provvederà anche a cancellare tutti i vari record correlati a tale violazione. In questa schermata vi è anche un bottone che consente l'aggiunta di una nuova violazione.
Nella vista compilazione verbale si avrà la possibilità di compilare il verbale completo di tutti i dati ed è possibile anche inserire più violazioni nel medesimo verbale. Le violazioni vengono recuperate dal database e mostrate tramite un menù a tendina.
In fine sulla navbar c'è il bottone del logout per cancellare il cookie di autenticazione.
Ho aggiunto un po' di stile css come richiesto dalla traccia.
Di seguito riporto la struttura delle tabelle del mio database.



CREATE TABLE [dbo].[ANAGRAFICA] (
    [Cognome]      NVARCHAR (50)  NOT NULL,
    [Nome]         NVARCHAR (50)  NOT NULL,
    [Indirizzo]    NVARCHAR (100) NULL,
    [Città]        NVARCHAR (50)  NULL,
    [CAP]          NVARCHAR (5)   NULL,
    [Cod_Fisc]     NVARCHAR (16)  NOT NULL,
    [Idanagrafica] INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ANAGRAFICA_Idanagrafica] PRIMARY KEY CLUSTERED ([Idanagrafica] ASC)
);


CREATE TABLE [dbo].[TIPO_VIOLAZIONE] (
    [Descrizione]       NVARCHAR (100)  NOT NULL,
    [Importo]           DECIMAL (10, 2) NOT NULL,
    [DecurtamentoPunti] INT             NOT NULL,
    [Idviolazione]      INT             IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_TIPO_VIOLAZIONE_Idviolazione] PRIMARY KEY CLUSTERED ([Idviolazione] ASC)
);


CREATE TABLE [dbo].[Utenti] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (255) NOT NULL,
    [Role]     NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[VERBALE] (
    [DataViolazione]          DATETIME2 (7)  NOT NULL,
    [IndirizzoViolazione]     NVARCHAR (100) NOT NULL,
    [Nominativo_Agente]       NVARCHAR (50)  NOT NULL,
    [DataTrascrizioneVerbale] DATETIME2 (7)  NULL,
    [Idanagrafica]            INT            NOT NULL,
    [Idverbale]               INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_VERBALE_Idverbale] PRIMARY KEY CLUSTERED ([Idverbale] ASC),
    CONSTRAINT [FK_VERBALE_Idanagrafica] FOREIGN KEY ([Idanagrafica]) REFERENCES [dbo].[ANAGRAFICA] ([Idanagrafica])
);


CREATE TABLE [dbo].[VERBALE_VIOLAZIONI] (
    [Idverbale]    INT NOT NULL,
    [Idviolazione] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Idverbale] ASC, [Idviolazione] ASC),
    CONSTRAINT [FK_VERBALE_VIOLAZIONI_Idviolazione] FOREIGN KEY ([Idviolazione]) REFERENCES [dbo].[TIPO_VIOLAZIONE] ([Idviolazione]),
    CONSTRAINT [FK_VERBALE_VIOLAZIONI_Idverbale] FOREIGN KEY ([Idverbale]) REFERENCES [dbo].[VERBALE] ([Idverbale])
);

