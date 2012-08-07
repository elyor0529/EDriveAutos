

CREATE PROCEDURE [dbo].[Nop_DiscountUsageHistoryDelete]
(
	@DiscountUsageHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_DiscountUsageHistory]
	WHERE
		DiscountUsageHistoryID = @DiscountUsageHistoryID
END
