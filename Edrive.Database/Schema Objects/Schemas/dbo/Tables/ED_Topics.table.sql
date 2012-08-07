CREATE TABLE [dbo].[ED_Topics] (
    [TopicId]      TINYINT        IDENTITY (1, 1) NOT NULL,
    [TopicTitle]   VARCHAR (100)  NOT NULL,
    [TopicContent] NVARCHAR (MAX) NULL,
    [CreatedBy]    INT            NOT NULL,
    [CreatedOn]    DATETIME       NOT NULL,
    [UpdatedBy]    INT            NOT NULL,
    [UpdatedOn]    DATETIME       NOT NULL
);

