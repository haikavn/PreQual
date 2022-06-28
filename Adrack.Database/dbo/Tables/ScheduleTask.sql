CREATE TABLE [dbo].[ScheduleTask] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [ServiceType]     VARCHAR (300) NOT NULL,
    [Seconds]         INT           NOT NULL,
    [Enabled]         BIT           NOT NULL,
    [StopOnError]     BIT           NOT NULL,
    [MachineName]     VARCHAR (200) NULL,
    [MachineDuration] DATETIME      NULL,
    [LastStart]       DATETIME      NULL,
    [LastEnd]         DATETIME      NULL,
    [LastSuccess]     DATETIME      NULL,
    [Description]     VARCHAR (200) NULL,
    CONSTRAINT [PK_ScheduleTask] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);













