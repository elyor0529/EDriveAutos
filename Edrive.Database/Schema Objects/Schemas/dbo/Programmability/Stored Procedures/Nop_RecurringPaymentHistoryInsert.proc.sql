

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentHistoryInsert]
(
	@RecurringPaymentHistoryID int = NULL output,
	@RecurringPaymentID int,
	@OrderID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_RecurringPaymentHistory]
	(
		[RecurringPaymentID],
		[OrderID],
		[CreatedOn]
	)
	VALUES
	(
		@RecurringPaymentID,
		@OrderID,
		@CreatedOn
	)

	set @RecurringPaymentHistoryID=SCOPE_IDENTITY()
END
