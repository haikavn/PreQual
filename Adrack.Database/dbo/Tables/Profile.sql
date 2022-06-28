CREATE TABLE [dbo].[Profile] (
    [Id]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserId]     BIGINT          NOT NULL,
    [FirstName]  NVARCHAR (150)  NULL,
    [MiddleName] NVARCHAR (150)  NULL,
    [LastName]   NVARCHAR (150)  NULL,
    [Phone]      VARCHAR (50)    NULL,
    [CellPhone]  VARCHAR (50)    NULL,
    [Summary]    NVARCHAR (1000) NULL,
    [JobTitle] NVARCHAR(150) NULL, 
    [CompanyName] NVARCHAR(150) NULL, 
    [CompanyWebSite] NVARCHAR(MAX) NULL, 
    [VerticalId] BIGINT NULL, 
    CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Profile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);














GO
CREATE NONCLUSTERED INDEX [NIX_Profile_UserId]
    ON [dbo].[Profile]([UserId] ASC) WITH (FILLFACTOR = 90);

