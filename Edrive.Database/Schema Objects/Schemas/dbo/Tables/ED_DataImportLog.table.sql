CREATE TABLE [dbo].[ED_DataImportLog] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [VIN]         VARCHAR (50)   NOT NULL,
    [FileName]    VARCHAR (50)   NOT NULL,
    [Status]      INT            NOT NULL,
    [Description] VARCHAR (1000) NULL,
    [CreatedDate] DATETIME       NOT NULL
);

