CREATE TABLE [dbo].[Language] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [Culture]      VARCHAR (15)  NOT NULL,
    [CultureId]    INT           NOT NULL,
    [Published]    BIT           NOT NULL,
    [Rtl]          BIT           NOT NULL,
    [DisplayOrder] INT           NOT NULL,
    [Description]  VARCHAR (200) NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);














GO
CREATE UNIQUE NONCLUSTERED INDEX [UNIX_Language_Name_Culture_CultureId]
    ON [dbo].[Language]([Name] ASC, [Culture] ASC, [CultureId] ASC) WITH (FILLFACTOR = 90);

