CREATE TABLE [dbo].[ED_ExpertDocuments] (
    [EDId]         INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (150) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [DocumentPath] NVARCHAR (150) NOT NULL,
    [IsActive]     BIT            NOT NULL,
    [CreatedOn]    DATETIME       NOT NULL,
    [UpdatedOn]    DATETIME       NULL
);

