CREATE TABLE [dbo].[AttachedChannel]
(
    [AffiliateChannelId] BIGINT NOT NULL, 
    [BuyerChannelId] BIGINT NOT NULL, 
    CONSTRAINT [PK_AttachedChannel] PRIMARY KEY ([AffiliateChannelId], [BuyerChannelId]), 
    CONSTRAINT [FK_AttachedChannel_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [AffiliateChannel]([Id]), 
    CONSTRAINT [FK_AttachedChannel_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [BuyerChannel]([Id]) 
 )