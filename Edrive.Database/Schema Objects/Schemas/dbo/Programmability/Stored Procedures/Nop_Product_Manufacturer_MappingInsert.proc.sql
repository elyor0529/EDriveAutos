

CREATE PROCEDURE [dbo].[Nop_Product_Manufacturer_MappingInsert]
(
	@ProductManufacturerID int = NULL output,
	@ProductID int,	
	@ManufacturerID int,
	@IsFeaturedProduct	bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Product_Manufacturer_Mapping]
	(
		ProductID,
		ManufacturerID,
		IsFeaturedProduct,
		DisplayOrder
	)
	VALUES
	(
		@ProductID,
		@ManufacturerID,
		@IsFeaturedProduct,
		@DisplayOrder
	)

	set @ProductManufacturerID=SCOPE_IDENTITY()
END
