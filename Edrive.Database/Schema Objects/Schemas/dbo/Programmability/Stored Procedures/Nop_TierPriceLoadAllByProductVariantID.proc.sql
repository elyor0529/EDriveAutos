

CREATE PROCEDURE [dbo].[Nop_TierPriceLoadAllByProductVariantID]
(
	@ProductVariantID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TierPrice]
	WHERE
		ProductVariantID=@ProductVariantID
	ORDER BY Quantity
END
