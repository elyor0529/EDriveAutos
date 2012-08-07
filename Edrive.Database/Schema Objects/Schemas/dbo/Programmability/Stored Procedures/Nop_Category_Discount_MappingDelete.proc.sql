

CREATE PROCEDURE [dbo].[Nop_Category_Discount_MappingDelete]
(
	@CategoryID int,
	@DiscountID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Category_Discount_Mapping]
	WHERE
		CategoryID = @CategoryID and DiscountID=@DiscountID
END
