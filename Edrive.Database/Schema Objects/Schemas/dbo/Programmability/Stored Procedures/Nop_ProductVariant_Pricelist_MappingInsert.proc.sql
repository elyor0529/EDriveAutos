

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Pricelist_MappingInsert]
(
	@ProductVariantID int,
	@PricelistID int,
	@PriceAdjustmentTypeID int,
	@PriceAdjustment money,
	@UpdatedOn datetime,
	@ProductVariantPricelistID int OUTPUT
)
AS
BEGIN
	INSERT INTO [Nop_ProductVariant_Pricelist_Mapping] 
	(
		[ProductVariantID],
		[PricelistID],
		[PriceAdjustmentTypeID],
		[PriceAdjustment],
		[UpdatedOn]
	) 
	VALUES 
	(
		@ProductVariantID,
		@PricelistID,
		@PriceAdjustmentTypeID,
		@PriceAdjustment,
		@UpdatedOn
	)

	SET @ProductVariantPricelistID = SCOPE_IDENTITY()
END
