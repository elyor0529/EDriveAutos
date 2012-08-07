CREATE TABLE [dbo].[ED_Testimonials] (
    [TId]       INT            IDENTITY (1, 1) NOT NULL,
    [TContent]  NVARCHAR (MAX) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [Address]   NVARCHAR (MAX) NOT NULL,
    [PictureId] INT            NOT NULL,
    [IsActive]  BIT            NOT NULL,
    [CreatedOn] DATETIME       NOT NULL,
    [UpdatedOn] DATETIME       NULL
);

