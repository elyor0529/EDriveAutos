

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueInsert]
(
	@ProductVariantAttributeValueID int = NULL output,
	@ProductVariantAttributeID int,
	@Name nvarchar (100),
	@PriceAdjustment money,
	@WeightAdjustment decimal(18, 4),
	@IsPreSelected bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariantAttributeValue]
	(
		[ProductVariantAttributeID],
		[Name],
		[PriceAdjustment],
		[WeightAdjustment],
		[IsPreSelected],
		[DisplayOrder]
	)
	VALUES
	(
		@ProductVariantAttributeID,
		@Name,
		@PriceAdjustment,
		@WeightAdjustment,
		@IsPreSelected,
		@DisplayOrder
	)

	set @ProductVariantAttributeValueID=SCOPE_IDENTITY()
END
