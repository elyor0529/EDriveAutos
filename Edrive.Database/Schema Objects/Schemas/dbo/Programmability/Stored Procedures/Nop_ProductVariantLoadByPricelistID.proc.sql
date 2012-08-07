

CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByPricelistID]
(
	@PricelistID int
)
AS
BEGIN
	SELECT pv.*
	FROM
		[Nop_ProductVariant] pv
		INNER JOIN [Nop_ProductVariant_Pricelist_Mapping] plm
		ON pv.ProductVariantID = plm.ProductVariantID
	WHERE
		plm.[PricelistID] = @PricelistID
END
