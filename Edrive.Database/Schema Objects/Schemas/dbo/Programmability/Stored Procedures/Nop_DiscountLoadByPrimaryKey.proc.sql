

CREATE PROCEDURE [dbo].[Nop_DiscountLoadByPrimaryKey]
(
	@DiscountID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Discount]
	WHERE
		DiscountID=@DiscountID
END
