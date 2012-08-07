

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentByPrimaryKey]
(
	@RecurringPaymentID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_RecurringPayment]
	WHERE
		([RecurringPaymentID] = @RecurringPaymentID)
END
