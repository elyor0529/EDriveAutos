

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentHistoryDelete]
(
	@RecurringPaymentHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_RecurringPaymentHistory]
	WHERE
		RecurringPaymentHistoryID = @RecurringPaymentHistoryID
END
