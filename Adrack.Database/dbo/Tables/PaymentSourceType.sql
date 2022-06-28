CREATE TABLE [dbo].[PaymentSourceType] (
    [Id]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50)  NULL,
    [Fields] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_PaymentSource] PRIMARY KEY CLUSTERED ([Id] ASC)
);

