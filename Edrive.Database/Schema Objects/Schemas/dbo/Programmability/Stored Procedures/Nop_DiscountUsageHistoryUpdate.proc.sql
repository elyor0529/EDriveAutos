

CREATE PROCEDURE [dbo].[Nop_DiscountUsageHistoryUpdate]
(
	@DiscountUsageHistoryID int,
	@DiscountID int,
	@CustomerID int,
	@OrderID int,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_DiscountUsageHistory]
	SET
		DiscountID=@DiscountID,
		CustomerID = @CustomerID,
		OrderID = @OrderID,
		CreatedOn = @CreatedOn
	WHERE
		DiscountUsageHistoryID=@DiscountUsageHistoryID
END
