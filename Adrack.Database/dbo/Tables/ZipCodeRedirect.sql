CREATE TABLE [dbo].[ZipCodeRedirect] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ZipCode]        VARCHAR (MAX)  NULL,
    [RedirectUrl]    VARCHAR (500)  NULL,
    [BuyerChannelId] BIGINT         NULL,
    [Title]          VARCHAR (500)  NULL,
    [Description]    VARCHAR (3000) NULL,
    [Address]        VARCHAR (1000) NULL,
    CONSTRAINT [PK_ZipCodeRedirect] PRIMARY KEY CLUSTERED ([Id] ASC)
);





