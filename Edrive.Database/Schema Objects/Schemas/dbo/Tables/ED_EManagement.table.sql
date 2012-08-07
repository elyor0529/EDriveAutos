CREATE TABLE [dbo].[ED_EManagement] (
    [ManagementID] INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (250) NOT NULL,
    [ImageID]      INT            NULL,
    [ShortDesc]    NVARCHAR (MAX) NOT NULL,
    [DisplayOrder] INT            NOT NULL,
    [Published]    BIT            NOT NULL,
    [Deleted]      BIT            NULL,
    [CreatedOn]    DATETIME       NOT NULL,
    [UpdatedOn]    DATETIME       NULL
);

