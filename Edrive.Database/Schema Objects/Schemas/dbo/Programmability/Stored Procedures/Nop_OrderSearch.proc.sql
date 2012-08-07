

CREATE PROCEDURE [dbo].[Nop_OrderSearch]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@CustomerEmail nvarchar(255) = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		o.*
	FROM [Nop_Order] o
	LEFT OUTER JOIN [Nop_Customer] c ON o.CustomerID = c.CustomerID
	WHERE
		(@CustomerEmail IS NULL or LEN(@CustomerEmail)=0 or (c.Email like '%' + COALESCE(@CustomerEmail,c.Email) + '%')) and
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) and
		(o.Deleted=0)
	ORDER BY o.CreatedOn desc
END
