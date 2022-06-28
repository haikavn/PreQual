CREATE TABLE [dbo].[Navigation] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [ParentId]     BIGINT        NOT NULL,
    [Layout]       VARCHAR (100) NOT NULL,
    [Key]          VARCHAR (250) NOT NULL,
    [Controller]   VARCHAR (100) NULL,
    [Action]       VARCHAR (100) NULL,
    [Permission]   VARCHAR (100) NOT NULL,
    [HtmlClass]    VARCHAR (200) NULL,
    [Url]          VARCHAR (300) NULL,
    [ImageUrl]     VARCHAR (300) NULL,
    [Published]    BIT           NOT NULL,
    [Deleted]      BIT           NOT NULL,
    [DisplayOrder] INT           NOT NULL,
    [Color]        VARCHAR (50)  NULL,
    CONSTRAINT [PK_Navigation] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);













