CREATE TABLE [dbo].[Setting] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Key]         VARCHAR (250)   NOT NULL,
    [Value]       NVARCHAR (1000) NOT NULL,
    [Description] VARCHAR (200)   NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);












GO


