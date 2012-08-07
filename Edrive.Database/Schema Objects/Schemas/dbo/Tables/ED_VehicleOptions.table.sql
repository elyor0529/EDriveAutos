CREATE TABLE [dbo].[ED_VehicleOptions] (
    [VehicleOptionId]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [VehicleOptionName] NVARCHAR (MAX) NOT NULL,
    [DisplayOrder]      INT            NULL
);

