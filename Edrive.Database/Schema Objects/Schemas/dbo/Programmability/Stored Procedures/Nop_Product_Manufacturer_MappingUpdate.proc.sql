

CREATE PROCEDURE [dbo].[Nop_Product_Manufacturer_MappingUpdate]
(
	@ProductManufacturerID int,
	@ProductID int,	
	@ManufacturerID int,
	@IsFeaturedProduct	bit,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_Product_Manufacturer_Mapping]
	SET
		ProductID=@ProductID,
		ManufacturerID=@ManufacturerID,
		IsFeaturedProduct=@IsFeaturedProduct,
		DisplayOrder=@DisplayOrder
	WHERE
		ProductManufacturerID = @ProductManufacturerID

END
