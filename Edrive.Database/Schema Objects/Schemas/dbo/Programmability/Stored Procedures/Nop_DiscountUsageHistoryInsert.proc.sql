

CREATE PROCEDURE [dbo].[Nop_DiscountUsageHistoryInsert]
(
	@DiscountUsageHistoryID int = NULL output,
	@DiscountID int,
	@CustomerID int,
	@OrderID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_DiscountUsageHistory]
	(
		[DiscountID],
		[CustomerID],
		[OrderID],
		[CreatedOn]
	)
	VALUES
	(
		@DiscountID,
		@CustomerID,
		@OrderID,
		@CreatedOn
	)

	set @DiscountUsageHistoryID=SCOPE_IDENTITY()
END
