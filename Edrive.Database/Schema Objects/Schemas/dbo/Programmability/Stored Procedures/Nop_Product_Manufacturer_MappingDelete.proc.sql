

CREATE PROCEDURE [dbo].[Nop_Product_Manufacturer_MappingDelete]
(
	@ProductManufacturerID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Product_Manufacturer_Mapping]
	WHERE
		ProductManufacturerID = @ProductManufacturerID
END
