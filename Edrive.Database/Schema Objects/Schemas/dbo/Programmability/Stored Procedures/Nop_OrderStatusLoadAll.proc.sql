

CREATE PROCEDURE [dbo].[Nop_OrderStatusLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderStatus]
	ORDER BY OrderStatusID
END
