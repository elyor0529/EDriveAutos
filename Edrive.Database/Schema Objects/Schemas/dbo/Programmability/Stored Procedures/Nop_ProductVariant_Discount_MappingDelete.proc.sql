

CREATE PROCEDURE [dbo].[Nop_ProductVariant_Discount_MappingDelete]
(
	@ProductVariantID int,
	@DiscountID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductVariant_Discount_Mapping]
	WHERE
		ProductVariantID = @ProductVariantID and DiscountID=@DiscountID
END
