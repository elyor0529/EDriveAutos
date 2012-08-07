

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeCombinationUpdate]
(
	@ProductVariantAttributeCombinationID int,
	@ProductVariantID int,
	@AttributesXML xml,
	@StockQuantity int,
	@AllowOutOfStockOrders bit
)
AS
BEGIN

	UPDATE [Nop_ProductVariantAttributeCombination]
	SET
		[ProductVariantID] = @ProductVariantID,
		[AttributesXML] = @AttributesXML,
		[StockQuantity] = @StockQuantity,
		[AllowOutOfStockOrders] = @AllowOutOfStockOrders
	WHERE
		ProductVariantAttributeCombinationID = @ProductVariantAttributeCombinationID
END
