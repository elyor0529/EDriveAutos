

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Pricelist_MappingLoadByProductVariantIDAndPricelistID]
(
	@ProductVariantID int, 
	@PricelistID int
)
AS
BEGIN
	SELECT *
	FROM
		[Nop_ProductVariant_Pricelist_Mapping]
	WHERE
		[ProductVariantID] = @ProductVariantID
		AND
		[PricelistID] = @PricelistID
END
