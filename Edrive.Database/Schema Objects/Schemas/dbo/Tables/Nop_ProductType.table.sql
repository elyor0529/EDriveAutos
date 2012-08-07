CREATE TABLE [dbo].[Nop_ProductType] (
    [ProductTypeID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100) NOT NULL,
    [DisplayOrder]  INT            NULL,
    [CreatedOn]     DATETIME       NOT NULL,
    [UpdatedOn]     DATETIME       NOT NULL
);

