

CREATE PROCEDURE [dbo].[Nop_OrderIncompleteReport]
(
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int
)
AS
BEGIN

	SELECT
		SUM(o.OrderTotal) [Total],
		COUNT(o.OrderID) [Count]
	FROM Nop_Order o
	WHERE 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) AND
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) AND
		(@ShippingStatusID IS NULL or @ShippingStatusID=0 or o.ShippingStatusID = @ShippingStatusID) AND
		o.Deleted=0

END
