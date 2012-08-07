

CREATE PROCEDURE [dbo].[Nop_DiscountRestrictionInsert]
(
	@ProductVariantID int,
	@DiscountID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_DiscountRestriction] WHERE ProductVariantID=@ProductVariantID and DiscountID=@DiscountID)
	INSERT
		INTO [Nop_DiscountRestriction]
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
