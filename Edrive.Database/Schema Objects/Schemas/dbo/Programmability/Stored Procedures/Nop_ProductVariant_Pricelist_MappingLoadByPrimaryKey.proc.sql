

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Pricelist_MappingLoadByPrimaryKey]
(
	@ProductVariantPricelistID int
)
AS
BEGIN
	SELECT *
	FROM
		[Nop_ProductVariant_Pricelist_Mapping]
	WHERE
		[ProductVariantPricelistID] = @ProductVariantPricelistID
END
