CREATE TABLE [dbo].[User] (
    [Id]                         BIGINT           IDENTITY (1, 1) NOT NULL,
    [ParentId]                   BIGINT           CONSTRAINT [DF_User_ParentId] DEFAULT ((0)) NOT NULL,
    [UserTypeId]                 BIGINT           NOT NULL,
    [GuId]                       VARCHAR (36)     NULL,
    [Username]                   VARCHAR (50)     NULL,
    [Email]                      VARCHAR (100)    NULL,
    [Password]                   VARCHAR (100)    NULL,
    [SaltKey]                    VARCHAR (50)     NULL,
    [Active]                     BIT              NOT NULL,
    [LockedOut]                  BIT              NOT NULL,
    [Deleted]                    BIT              NOT NULL,
    [BuiltIn]                    BIT              NOT NULL,
    [BuiltInName]                VARCHAR (100)    NULL,
    [RegistrationDate]           DATETIME         NOT NULL,
    [LoginDate]                  DATETIME         NOT NULL,
    [ActivityDate]               DATETIME         NOT NULL,
    [PasswordChangedDate]        DATETIME         NULL,
    [LockoutDate]                DATETIME         NULL,
    [IpAddress]                  VARCHAR (15)     NULL,
    [FailedPasswordAttemptCount] INT              NULL,
    [Comment]                    VARCHAR (200)    NULL,
    [DepartmentId]               BIGINT           NULL,
    [MenuType]                   SMALLINT         NULL,
    [MaskEmail]                  BIT              NULL,
    [ValidateOnLogin]            BIT              NULL,
    [ChangePassOnLogin]          BIT              NULL,
    [TimeZone]                   VARCHAR (100)    NULL,
    [RemoteLoginGuid]            UNIQUEIDENTIFIER NULL,
    [ContactEmail]               VARCHAR (100)    NULL,
    [UserType] SMALLINT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_User_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([Id])
);
















































GO
CREATE NONCLUSTERED INDEX [NIX_User_UserTypeId]
    ON [dbo].[User]([UserTypeId] ASC) WITH (FILLFACTOR = 90);

