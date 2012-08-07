CREATE TABLE [dbo].[ED_CarfaxLogDetail] (
    [CarFax_logID] INT            IDENTITY (1, 1) NOT NULL,
    [LogMsg]       NVARCHAR (500) NOT NULL,
    [Status]       INT            NOT NULL,
    [Success]      INT            NULL,
    [CreateBy]     INT            NOT NULL,
    [CreateOn]     DATETIME       NOT NULL
);

