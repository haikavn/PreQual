CREATE TABLE [dbo].[AffiliateUsers] (
    [UserId]      BIGINT NULL,
    [AffiliateId] BIGINT NULL,
    CONSTRAINT [FK_AffiliateUsers_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_AffiliateUsers_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateUsers_UserId]
    ON [dbo].[AffiliateUsers]([UserId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateUsers_AffiliateId]
    ON [dbo].[AffiliateUsers]([AffiliateId] ASC) WITH (FILLFACTOR = 90);

