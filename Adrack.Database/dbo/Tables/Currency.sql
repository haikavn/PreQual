CREATE TABLE [dbo].[Currency] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)    NOT NULL,
    [Code]         VARCHAR (3)     NOT NULL,
    [Rate]         DECIMAL (18, 4) NOT NULL,
    [Symbol]       NVARCHAR (10)   NOT NULL,
    [Published]    BIT             NOT NULL,
    [DisplayOrder] INT             NOT NULL,
    [CreatedOn]    DATETIME        CONSTRAINT [DF_Currency_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedOn]    DATETIME        NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);
















GO
CREATE UNIQUE NONCLUSTERED INDEX [UNIX_Currency_Name_Code]
    ON [dbo].[Currency]([Name] ASC, [Code] ASC) WITH (FILLFACTOR = 90);

