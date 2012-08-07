

CREATE PROCEDURE [dbo].[Nop_DiscountRestrictionDelete]
(
	@ProductVariantID int,
	@DiscountID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_DiscountRestriction]
	WHERE
		ProductVariantID = @ProductVariantID and DiscountID=@DiscountID
END
