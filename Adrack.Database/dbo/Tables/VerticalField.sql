CREATE TABLE [dbo].[VerticalField] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Type]        TINYINT       NOT NULL,
    [VerticalId]  BIGINT        NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [IsRequired]  BIT           NOT NULL,
    CONSTRAINT [PK_VerticalField] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VerticalField_Vertical] FOREIGN KEY ([VerticalId]) REFERENCES [dbo].[Vertical] ([Id])
);

