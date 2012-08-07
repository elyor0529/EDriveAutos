

CREATE PROCEDURE [dbo].[Nop_Category_Discount_MappingInsert]
(
	@CategoryID int,
	@DiscountID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_Category_Discount_Mapping] WHERE CategoryID=@CategoryID and DiscountID=@DiscountID)
	INSERT
		INTO [Nop_Category_Discount_Mapping]
		(
			CategoryID,
			DiscountID
		)
		VALUES
		(
			@CategoryID,
			@DiscountID
		)
END
