

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Discount_MappingInsert]
(
	@ProductVariantID int,
	@DiscountID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_ProductVariant_Discount_Mapping] WHERE ProductVariantID=@ProductVariantID and DiscountID=@DiscountID)
	INSERT
		INTO [Nop_ProductVariant_Discount_Mapping]
		(
			ProductVariantID,
			DiscountID
		)
		VALUES
		(
			@ProductVariantID,
			@DiscountID
		)
END
