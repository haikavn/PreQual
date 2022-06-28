CREATE TABLE [dbo].[RedirectUrl] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created]       DATETIME       NULL,
    [LeadId]        BIGINT         NULL,
    [Url]           VARCHAR (300)  NULL,
    [Clicked]       BIT            NULL,
    [ClickDate]     DATETIME       NULL,
    [NavigationKey] VARCHAR (50)   NULL,
    [Ip]            VARCHAR (50)   NULL,
    [Device]        VARCHAR (255)  NULL,
    [Browser]       VARCHAR (255)  NULL,
    [OS]            VARCHAR (255)  NULL,
    [Title]         VARCHAR (500)  NULL,
    [Description]   VARCHAR (3000) NULL,
    [Address]       VARCHAR (1000) NULL,
    [ZipCode]       VARCHAR (10)   NULL,
    CONSTRAINT [PK_RedirectUrl] PRIMARY KEY CLUSTERED ([Id] ASC)
);








GO
CREATE NONCLUSTERED INDEX [NIX_RedirectUrl_Clicked_Ip_LeadId]
    ON [dbo].[RedirectUrl]([Clicked] ASC, [Ip] ASC, [LeadId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_IndexName]
    ON [dbo].[RedirectUrl]([LeadId] ASC);

