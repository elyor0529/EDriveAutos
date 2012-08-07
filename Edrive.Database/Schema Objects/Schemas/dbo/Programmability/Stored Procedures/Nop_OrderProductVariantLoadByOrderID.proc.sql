

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantLoadByOrderID]
(
	@OrderID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderProductVariant]
	WHERE
		OrderID = @OrderID
END
