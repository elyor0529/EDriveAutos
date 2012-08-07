CREATE TABLE [dbo].[ED_Partners] (
    [PartnerId]  INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (100) NOT NULL,
    [LastName]   NVARCHAR (100) NOT NULL,
    [Phone]      NVARCHAR (20)  NULL,
    [Email]      NVARCHAR (100) NOT NULL,
    [Company]    NVARCHAR (50)  NOT NULL,
    [Website]    NVARCHAR (100) NOT NULL,
    [Comments]   NVARCHAR (MAX) NULL,
    [PictureId]  INT            NULL,
    [IsApproved] BIT            NULL,
    [IsActive]   BIT            NOT NULL,
    [CreatedOn]  DATETIME       NOT NULL,
    [UpdatedOn]  DATETIME       NULL
);

