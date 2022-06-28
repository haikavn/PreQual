CREATE TABLE [dbo].[CallCenterSetting] (
    [Id]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [BuyerId]         BIGINT       NULL,
    [StoreId]         BIGINT       NULL,
    [CallCenterPhone] VARCHAR (50) NULL,
    [Action]          SMALLINT     NULL,
    [CanRecord]       BIT          NULL,
    [HasVoip]         BIT          NULL,
    [RecordDelay]     INT          NULL,
    CONSTRAINT [PK_CallCenterSetting] PRIMARY KEY CLUSTERED ([Id] ASC)
);





