CREATE TABLE [dbo].[PaymentSource] (
    [Id]                  BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]             DATETIME NULL,
    [ParentId]            BIGINT   NULL,
    [UserTypeId]          BIGINT   NULL,
    [PaymentSourceTypeId] BIGINT   NULL,
    CONSTRAINT [PK_PaymentSource_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

