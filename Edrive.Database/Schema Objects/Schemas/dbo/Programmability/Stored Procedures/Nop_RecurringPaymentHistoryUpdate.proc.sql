

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentHistoryUpdate]
(
	@RecurringPaymentHistoryID int,
	@RecurringPaymentID int,
	@OrderID int,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_RecurringPaymentHistory]
	SET
		RecurringPaymentID=@RecurringPaymentID,
		OrderID = @OrderID,
		CreatedOn = @CreatedOn
	WHERE
		RecurringPaymentHistoryID=@RecurringPaymentHistoryID
END
