

CREATE PROCEDURE [dbo].[Nop_GiftCardUsageHistoryInsert]
(
	@GiftCardUsageHistoryID int = NULL output,
	@GiftCardID int,
	@CustomerID int,
	@OrderID int,
	@UsedValue money,
	@UsedValueInCustomerCurrency money,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_GiftCardUsageHistory]
	(
		[GiftCardID],
		[CustomerID],
		[OrderID],
		[UsedValue],
		[UsedValueInCustomerCurrency],
		[CreatedOn]
	)
	VALUES
	(
		@GiftCardID,
		@CustomerID,
		@OrderID,
		@UsedValue,
		@UsedValueInCustomerCurrency,
		@CreatedOn
	)

	set @GiftCardUsageHistoryID=SCOPE_IDENTITY()
END
