

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Pricelist_MappingUpdate]
(
	@ProductVariantPricelistID int,
	@ProductVariantID int,
	@PricelistID int,
	@PriceAdjustmentTypeID int,
	@PriceAdjustment money,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductVariant_Pricelist_Mapping] 
	SET
		[ProductVariantID] = @ProductVariantID,
		[PricelistID] = @PricelistID,
		[PriceAdjustmentTypeID] = @PriceAdjustmentTypeID,
		[PriceAdjustment] = @PriceAdjustment,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[ProductVariantPricelistID] = @ProductVariantPricelistID
END
