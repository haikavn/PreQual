CREATE TABLE [dbo].[LeadFieldsContent] (
    [Id]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [LeadId]        BIGINT          NULL,
    [StringValue]   VARCHAR (200)   NULL,
    [IntValue]      INT             NULL,
    [DecimalValue]  DECIMAL (18, 2) NULL,
    [DateTimeValue] DATETIME        NULL,
    [FieldName]     VARCHAR (50)    NULL,
    CONSTRAINT [PK_LeadFieldsContent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

