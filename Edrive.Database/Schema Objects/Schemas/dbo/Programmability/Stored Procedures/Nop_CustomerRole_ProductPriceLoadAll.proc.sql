

CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceLoadAll]
(
	@ProductVariantID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerRole_ProductPrice]
	WHERE
		ProductVariantID= @ProductVariantID
END
