

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueUpdate]
(
	@ProductVariantAttributeValueID int,
	@ProductVariantAttributeID int,
	@Name nvarchar (100),
	@PriceAdjustment money,
	@WeightAdjustment decimal(18, 4),
	@IsPreSelected bit,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_ProductVariantAttributeValue]
	SET
		ProductVariantAttributeID=@ProductVariantAttributeID,
		[Name]=@Name,
		PriceAdjustment=@PriceAdjustment,
		WeightAdjustment=@WeightAdjustment,
		IsPreSelected=@IsPreSelected,
		DisplayOrder=@DisplayOrder
	WHERE
		ProductVariantAttributeValueID = @ProductVariantAttributeValueID
END
