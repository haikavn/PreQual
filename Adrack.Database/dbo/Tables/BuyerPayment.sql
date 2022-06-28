CREATE TABLE [dbo].[BuyerPayment] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [BuyerId]       BIGINT        NOT NULL,
    [PaymentDate]   DATETIME      NULL,
    [Amount]        FLOAT (53)    NULL,
    [Note]          VARCHAR (255) NULL,
    [Created]       DATETIME      NULL,
    [UserId]        BIGINT        NOT NULL,
    [PaymentMethod] SMALLINT      NULL,
    CONSTRAINT [PK_payments] PRIMARY KEY CLUSTERED ([Id] ASC)
);





