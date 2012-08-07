

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantLoadAll]
(
	@OrderID int,
	@CustomerID int,
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int,
	@LoadDownloableProductsOnly bit = NULL
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		opv.*
	FROM [Nop_OrderProductVariant] opv
	INNER JOIN [Nop_Order] o ON opv.OrderID=o.OrderID
	INNER JOIN [Nop_ProductVariant] pv ON opv.ProductVariantID=pv.ProductVariantID
	WHERE
		(@OrderID IS NULL OR @OrderID=0 or o.OrderID = @OrderID) and
		(@CustomerID IS NULL OR @CustomerID=0 or o.CustomerID = @CustomerID) and
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) and
		((@LoadDownloableProductsOnly IS NULL OR @LoadDownloableProductsOnly = 0) OR (pv.IsDownload=1)) and
		(o.Deleted=0)		
	ORDER BY o.CreatedOn desc, [opv].OrderProductVariantID 
END
