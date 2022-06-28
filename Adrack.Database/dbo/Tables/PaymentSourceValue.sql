CREATE TABLE [dbo].[PaymentSourceValue] (
    [Id]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [FieldName]       VARCHAR (50) NULL,
    [FieldValue]      VARCHAR (50) NULL,
    [PaymentSourceId] BIGINT       NULL,
    CONSTRAINT [PK_PaymentSourceValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

