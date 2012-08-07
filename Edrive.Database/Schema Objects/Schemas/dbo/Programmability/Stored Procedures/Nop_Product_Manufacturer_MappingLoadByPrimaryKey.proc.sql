

CREATE PROCEDURE [dbo].[Nop_Product_Manufacturer_MappingLoadByPrimaryKey]
(
	@ProductManufacturerID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Product_Manufacturer_Mapping]
	WHERE
		ProductManufacturerID = @ProductManufacturerID
END
