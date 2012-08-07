

CREATE PROCEDURE [dbo].[Nop_GiftCardUsageHistoryLoadAll]
(
	@GiftCardID int,
	@CustomerID int,
	@OrderID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT * FROM [Nop_GiftCardUsageHistory]
	WHERE GiftCardUsageHistoryID IN 
		(
		SELECT DISTINCT gcuh.GiftCardUsageHistoryID
		FROM [Nop_GiftCardUsageHistory] gcuh WITH (NOLOCK)
		LEFT OUTER JOIN Nop_GiftCard gc with (NOLOCK) ON gcuh.GiftCardID=gc.GiftCardID
		LEFT OUTER JOIN Nop_OrderProductVariant opv with (NOLOCK) ON gc.PurchasedOrderProductVariantID=opv.OrderProductVariantID
		LEFT OUTER JOIN Nop_Order o with (NOLOCK) ON gcuh.OrderID=o.OrderID
		WHERE
				(
					o.Deleted=0
				)
				AND
				(
					@GiftCardID IS NULL OR @GiftCardID=0
					OR (gcuh.GiftCardID=@GiftCardID)
				)
				AND
				(
					@CustomerID IS NULL OR @CustomerID=0
					OR (gcuh.CustomerID=@CustomerID)
				)
				AND
				(
					@OrderID IS NULL OR @OrderID=0
					OR (gcuh.OrderID=@OrderID)
				)
		)
	ORDER BY CreatedOn, GiftCardUsageHistoryID
END
