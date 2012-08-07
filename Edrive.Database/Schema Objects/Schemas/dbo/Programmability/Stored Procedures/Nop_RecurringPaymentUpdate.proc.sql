

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentUpdate]
(
	@RecurringPaymentID int,
	@InitialOrderID int,
	@CycleLength int,
	@CyclePeriod int,
	@TotalCycles int,
	@StartDate datetime,
	@IsActive bit,
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_RecurringPayment]
	SET
		[InitialOrderID]=@InitialOrderID,
		[CycleLength]=@CycleLength,
		[CyclePeriod]=@CyclePeriod,
		[TotalCycles]=@TotalCycles,
		[StartDate]=@StartDate,
		[IsActive]=@IsActive,
		[Deleted]=@Deleted,
		[CreatedOn]=@CreatedOn
	WHERE
		[RecurringPaymentID] = @RecurringPaymentID

END
