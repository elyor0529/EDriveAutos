CREATE TABLE [dbo].[ED_EdriveProducts] (
    [EDProductId]      INT            IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (100) NOT NULL,
    [ShortDescription] NVARCHAR (255) NULL,
    [FullDescription]  NVARCHAR (MAX) NULL,
    [PictureId]        INT            NULL,
    [IsActive]         BIT            NOT NULL,
    [CreatedOn]        DATETIME       NOT NULL,
    [UpdatedOn]        DATETIME       NULL,
    [DisplayOrder]     INT            NULL
);

