
CREATE PROCEDURE [dbo].[Nop_GiftCardUsageHistoryUpdate]
(
	@GiftCardUsageHistoryID int,
	@GiftCardID int,
	@CustomerID int,
	@OrderID int,
	@UsedValue money,
	@UsedValueInCustomerCurrency money,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_GiftCardUsageHistory]
	SET
		[GiftCardID]=@GiftCardID,
		[CustomerID] = @CustomerID,
		[OrderID] = @OrderID,
		[UsedValue]= @UsedValue,
		[UsedValueInCustomerCurrency] = @UsedValueInCustomerCurrency,
		[CreatedOn] = @CreatedOn
	WHERE
		GiftCardUsageHistoryID=@GiftCardUsageHistoryID
END
