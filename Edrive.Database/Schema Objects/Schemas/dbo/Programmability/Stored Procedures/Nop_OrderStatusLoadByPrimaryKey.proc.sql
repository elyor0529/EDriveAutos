

CREATE PROCEDURE [dbo].[Nop_OrderStatusLoadByPrimaryKey]
(
	@OrderStatusID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderStatus]
	WHERE
		OrderStatusID = @OrderStatusID
END
