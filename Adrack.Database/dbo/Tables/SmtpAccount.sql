CREATE TABLE [dbo].[SmtpAccount] (
    [Id]                    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Email]                 VARCHAR (100) NOT NULL,
    [DisplayName]           VARCHAR (50)  NOT NULL,
    [Host]                  VARCHAR (50)  NOT NULL,
    [Port]                  INT           NOT NULL,
    [Username]              VARCHAR (50)  NOT NULL,
    [Password]              VARCHAR (50)  NOT NULL,
    [EnableSsl]             BIT           NOT NULL,
    [UseDefaultCredentials] BIT           NOT NULL,
    CONSTRAINT [PK_SmtpAccount] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);



