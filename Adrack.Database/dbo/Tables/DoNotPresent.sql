CREATE TABLE [dbo].[DoNotPresent] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [email]          VARCHAR (150) NULL,
    [ssn]            VARCHAR (50)  NULL,
    [phone]          VARCHAR (50)  NULL,
    [expirationdate] DATETIME      NULL,
    [buyerid]        BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);







GO
CREATE NONCLUSTERED INDEX [IX_DoNotPresent_Email_Ssn]
    ON [dbo].[DoNotPresent]([email] ASC, [ssn] ASC, [expirationdate] ASC);



