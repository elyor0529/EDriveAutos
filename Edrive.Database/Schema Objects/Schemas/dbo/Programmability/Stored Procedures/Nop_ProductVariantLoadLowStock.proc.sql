

CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadLowStock]
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductVariant]
	WHERE
		Deleted=0 and
		MinStockQuantity >= StockQuantity
	ORDER BY MinStockQuantity
END
