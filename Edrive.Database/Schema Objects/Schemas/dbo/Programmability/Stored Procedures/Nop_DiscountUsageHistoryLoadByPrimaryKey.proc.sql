

CREATE PROCEDURE [dbo].[Nop_DiscountUsageHistoryLoadByPrimaryKey]
(
	@DiscountUsageHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_DiscountUsageHistory]
	WHERE
		DiscountUsageHistoryID = @DiscountUsageHistoryID
END
