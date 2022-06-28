CREATE TABLE [dbo].[RegistrationRequest] (
    [Id]      BIGINT       IDENTITY (1, 1) NOT NULL,
    [Email]   VARCHAR (50) NULL,
    [Code]    VARCHAR (50) NULL,
    [Created] DATETIME     NULL,
    [Name]    VARCHAR (50) NULL,
    CONSTRAINT [PK_RegistrationRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

