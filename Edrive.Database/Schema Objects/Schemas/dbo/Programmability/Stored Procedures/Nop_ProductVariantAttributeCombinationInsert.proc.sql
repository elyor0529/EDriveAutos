

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeCombinationInsert]
(
	@ProductVariantAttributeCombinationID int = NULL output,
	@ProductVariantID int,
	@AttributesXML xml,
	@StockQuantity int,
	@AllowOutOfStockOrders bit
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariantAttributeCombination]
	(
		[ProductVariantID],
		[AttributesXML],
		[StockQuantity],
		[AllowOutOfStockOrders]
	)
	VALUES
	(	
		@ProductVariantID,
		@AttributesXML,
		@StockQuantity,
		@AllowOutOfStockOrders
	)

	set @ProductVariantAttributeCombinationID=SCOPE_IDENTITY()
END
