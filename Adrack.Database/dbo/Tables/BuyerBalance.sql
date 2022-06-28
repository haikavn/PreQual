CREATE TABLE [dbo].[BuyerBalance] (
    [Id]         BIGINT IDENTITY (1, 1) NOT NULL,
    [BuyerId]    BIGINT NULL,
    [SoldSum]    MONEY  NULL,
    [PaymentSum] MONEY  NULL,
    [Balance]    MONEY  NULL,
    [Credit]     MONEY  NULL,
    CONSTRAINT [PK_BuyerBanace] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [NIX_BuyerBalance_Balance_Credit_INCLUDE]
    ON [dbo].[BuyerBalance]([Balance] ASC, [Credit] ASC)
    INCLUDE([BuyerId]) WITH (FILLFACTOR = 90);

