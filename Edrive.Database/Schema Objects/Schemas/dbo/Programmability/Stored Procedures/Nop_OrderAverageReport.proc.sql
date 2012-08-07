

CREATE PROCEDURE [dbo].[Nop_OrderAverageReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT 
		SUM(o.OrderTotal) as SumOrders,
		COUNT(1) as CountOrders
	FROM [Nop_Order] o
	WHERE 
		(@StartTime is NULL or @StartTime <= o.CreatedOn) AND
		(@EndTime is NULL or @EndTime >= o.CreatedOn) AND
		o.OrderTotal > 0 AND 
		o.OrderStatusID=@OrderStatusID AND 
		o.Deleted=0	
END
