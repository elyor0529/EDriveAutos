

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Pricelist_MappingDelete]
(
	@ProductVariantPricelistID int
)
AS
BEGIN
	DELETE 
	FROM [Nop_ProductVariant_Pricelist_Mapping]
	WHERE
		[ProductVariantPricelistID] = @ProductVariantPricelistID
END
