

CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryInsert]
(
	@RewardPointsHistoryID int = NULL output,
	@CustomerID int,
	@OrderID int,
	@Points int,
	@PointsBalance int,
	@UsedAmount money,
	@UsedAmountInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@Message nvarchar(1000),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_RewardPointsHistory]
	(
		[CustomerID],
		[OrderID],
		[Points],
		[PointsBalance],
		[UsedAmount],
		[UsedAmountInCustomerCurrency],
		[CustomerCurrencyCode],
		[Message],
		[CreatedOn]
	)
	VALUES
	(
		@CustomerID,
		@OrderID,
		@Points,
		@PointsBalance,
		@UsedAmount,
		@UsedAmountInCustomerCurrency,
		@CustomerCurrencyCode,
		@Message,
		@CreatedOn
	)

	set @RewardPointsHistoryID=SCOPE_IDENTITY()
END
