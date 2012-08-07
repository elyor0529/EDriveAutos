

CREATE PROCEDURE [dbo].[Nop_ProductVariantRestrictedLoadDiscountID]
(
	@DiscountID int
)
AS
BEGIN
	SELECT pv.*
	FROM
		[Nop_ProductVariant] pv
		INNER JOIN [Nop_DiscountRestriction] dr
		ON pv.ProductVariantID = dr.ProductVariantID
	WHERE
		dr.[DiscountID] = @DiscountID
END
