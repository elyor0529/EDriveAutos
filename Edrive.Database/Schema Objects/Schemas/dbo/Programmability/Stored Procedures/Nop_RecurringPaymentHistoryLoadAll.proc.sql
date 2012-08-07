

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentHistoryLoadAll]
(
	@RecurringPaymentID int = NULL,
	@OrderID int = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT * FROM [Nop_RecurringPaymentHistory]
	WHERE RecurringPaymentHistoryID IN 
		(
		SELECT DISTINCT rph.RecurringPaymentHistoryID
		FROM [Nop_RecurringPaymentHistory] rph WITH (NOLOCK)
		INNER JOIN Nop_RecurringPayment rp with (NOLOCK) ON rph.RecurringPaymentID=rp.RecurringPaymentID
		LEFT OUTER JOIN Nop_Order o with (NOLOCK) ON rph.OrderID=o.OrderID
		WHERE
				(
					rp.Deleted=0 AND o.Deleted=0 
				)
				AND
				(
					@RecurringPaymentID IS NULL OR @RecurringPaymentID=0
					OR (rph.RecurringPaymentID=@RecurringPaymentID)
				)
				AND
				(
					@OrderID IS NULL OR @OrderID=0
					OR (rph.OrderID=@OrderID)
				)
		)
	ORDER BY CreatedOn, RecurringPaymentHistoryID
END
