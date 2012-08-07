

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentLoadAll]
(
	@ShowHidden		bit = 0,
	@CustomerID		int = NULL,
	@InitialOrderID	int = NULL,
	@InitialOrderStatusID int = NULL
)
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT * FROM [Nop_RecurringPayment]
	WHERE RecurringPaymentID IN 
		(
		SELECT DISTINCT rp.RecurringPaymentID
		FROM [Nop_RecurringPayment] rp WITH (NOLOCK)
		INNER JOIN Nop_Order o with (NOLOCK) ON rp.InitialOrderID=o.OrderID
		INNER JOIN Nop_Customer c with (NOLOCK) ON o.CustomerID=c.CustomerID
		WHERE
				(
					rp.Deleted=0 AND o.Deleted=0 AND c.Deleted=0
				)
				AND 
				(
					@ShowHidden = 1 OR rp.IsActive=1
				)
				AND
				(
					@CustomerID IS NULL OR @CustomerID=0
					OR (o.CustomerID=@CustomerID)
				)
				AND
				(
					@InitialOrderID IS NULL OR @InitialOrderID=0
					OR (rp.InitialOrderID=@InitialOrderID)
				)
				AND
				(
					@InitialOrderStatusID IS NULL OR @InitialOrderStatusID=0
					OR (o.OrderStatusID=@InitialOrderStatusID)
				)
		)
	ORDER BY StartDate, RecurringPaymentID
	
END
