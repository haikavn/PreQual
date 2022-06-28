CREATE TABLE [dbo].[LeadSensitiveData] (
    [Id]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [LeadId]  BIGINT         NULL,
    [Data1]   VARCHAR (1000) NULL,
    [Data2]   VARCHAR (1000) NULL,
    [Data3]   VARCHAR (1000) NULL,
    [Data4]   VARCHAR (1000) NULL,
    [Data5]   VARCHAR (1000) NULL,
    [Data7]   VARCHAR (1000) NULL,
    [Data8]   VARCHAR (1000) NULL,
    [Data9]   VARCHAR (1000) NULL,
    [Data10]  VARCHAR (1000) NULL,
    [Created] DATETIME       NULL,
    [Data6]   VARCHAR (1000) NULL,
    CONSTRAINT [PK_LeadSensitiveData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

