

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@BillingCountryID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT opv.ProductVariantID,
		(	
			select sum(opv2.PriceExclTax)
			from Nop_OrderProductVariant opv2
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or @StartTime <= o2.CreatedOn) and
				(@EndTime is NULL or @EndTime >= o2.CreatedOn) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(@BillingCountryID IS NULL or @BillingCountryID=0 or o2.BillingCountryID = @BillingCountryID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) PriceExclTax, 
		(
			select sum(opv2.Quantity)  
			from Nop_OrderProductVariant opv2 
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or @StartTime <= o2.CreatedOn) and
				(@EndTime is NULL or @EndTime >= o2.CreatedOn) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(@BillingCountryID IS NULL or @BillingCountryID=0 or o2.BillingCountryID = @BillingCountryID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) Total 
	FROM Nop_OrderProductVariant opv 
	INNER JOIN [Nop_Order] o 
	on o.OrderId = opv.OrderID
	WHERE
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@BillingCountryID IS NULL or @BillingCountryID=0 or o.BillingCountryID = @BillingCountryID) and
		(o.Deleted=0)

END
