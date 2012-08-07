CREATE TABLE [dbo].[ED_EGear] (
    [eGearID]      INT            IDENTITY (1, 1) NOT NULL,
    [ProductName]  NVARCHAR (255) NULL,
    [Price]        MONEY          NULL,
    [Qty]          INT            NULL,
    [ImageID]      INT            NULL,
    [ShortDesc]    NVARCHAR (MAX) NULL,
    [DisplayOrder] INT            NULL,
    [Published]    BIT            NULL,
    [Deleted]      BIT            NULL,
    [CreatedOn]    DATETIME       NULL,
    [UpdatedOn]    DATETIME       NULL
);

