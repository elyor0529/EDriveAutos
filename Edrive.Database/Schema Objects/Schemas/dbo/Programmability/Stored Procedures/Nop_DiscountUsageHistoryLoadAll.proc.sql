

CREATE PROCEDURE [dbo].[Nop_DiscountUsageHistoryLoadAll]
(
	@DiscountID int,
	@CustomerID int,
	@OrderID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT * FROM [Nop_DiscountUsageHistory]
	WHERE DiscountUsageHistoryID IN 
		(
		SELECT DISTINCT duh.DiscountUsageHistoryID
		FROM [Nop_DiscountUsageHistory] duh WITH (NOLOCK)
		LEFT OUTER JOIN Nop_Discount d with (NOLOCK) ON duh.DiscountID=d.DiscountID
		LEFT OUTER JOIN Nop_Customer c with (NOLOCK) ON duh.CustomerID=c.CustomerID
		LEFT OUTER JOIN Nop_Order o with (NOLOCK) ON duh.OrderID=o.OrderID
		WHERE
				(
					d.Deleted=0 AND c.Deleted=0 AND o.Deleted=0 
				)
				AND
				(
					@DiscountID IS NULL OR @DiscountID=0
					OR (duh.DiscountID=@DiscountID)
				)
				AND
				(
					@CustomerID IS NULL OR @CustomerID=0
					OR (duh.CustomerID=@CustomerID)
				)
				AND
				(
					@OrderID IS NULL OR @OrderID=0
					OR (duh.OrderID=@OrderID)
				)
		)
	ORDER BY CreatedOn, DiscountUsageHistoryID
END
