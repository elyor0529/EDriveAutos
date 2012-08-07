

CREATE PROCEDURE [dbo].[Nop_RecurringPaymentInsert]
(
	@RecurringPaymentID int = NULL output,
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
	INSERT
	INTO [Nop_RecurringPayment]
	(
		[InitialOrderID],
		[CycleLength],
		[CyclePeriod],
		[TotalCycles],
		[StartDate],
		[IsActive],
		[Deleted],
		[CreatedOn]
	)
	VALUES
	(
		@InitialOrderID,
		@CycleLength,
		@CyclePeriod,
		@TotalCycles,
		@StartDate,
		@IsActive,
		@Deleted,
		@CreatedOn
	)

	set @RecurringPaymentID=SCOPE_IDENTITY()
END
