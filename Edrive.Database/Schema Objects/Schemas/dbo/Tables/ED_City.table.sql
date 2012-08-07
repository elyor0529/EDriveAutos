CREATE TABLE [dbo].[ED_City] (
    [CityId]          INT            IDENTITY (1, 1) NOT NULL,
    [CountryID]       INT            NOT NULL,
    [StateProvinceID] INT            NOT NULL,
    [Name]            NVARCHAR (100) NOT NULL,
    [DisplayOrder]    INT            NOT NULL,
    [CityImageId]     INT            NULL,
    [CreatedOn]       DATETIME       NULL,
    [Deleted]         BIT            NULL,
    [UpdatedOn]       DATETIME       NULL,
    [ShowOnHomepage]  BIT            NULL
);

