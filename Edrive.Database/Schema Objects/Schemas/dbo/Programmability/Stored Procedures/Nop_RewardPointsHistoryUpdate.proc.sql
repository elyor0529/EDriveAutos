

CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryUpdate]
(
	@RewardPointsHistoryID int,
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
	UPDATE [Nop_RewardPointsHistory]
	SET
		[CustomerID] = @CustomerID,
		[OrderID] = @OrderID,
		[Points] = @Points,
		[PointsBalance] = @PointsBalance,
		[UsedAmount] = @UsedAmount,
		[UsedAmountInCustomerCurrency] = @UsedAmountInCustomerCurrency,
		[CustomerCurrencyCode] = @CustomerCurrencyCode,
		[Message] = @Message,
		[CreatedOn] = @CreatedOn
	WHERE
		RewardPointsHistoryID=@RewardPointsHistoryID
END
