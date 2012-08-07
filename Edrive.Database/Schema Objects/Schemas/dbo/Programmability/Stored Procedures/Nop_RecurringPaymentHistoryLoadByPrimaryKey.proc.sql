

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentHistoryLoadByPrimaryKey]
(
	@RecurringPaymentHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_RecurringPaymentHistory]
	WHERE
		RecurringPaymentHistoryID = @RecurringPaymentHistoryID
END
