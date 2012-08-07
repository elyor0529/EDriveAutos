

CREATE PROCEDURE [dbo].[Nop_GiftCardUsageHistoryLoadByPrimaryKey]
(
	@GiftCardUsageHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_GiftCardUsageHistory]
	WHERE
		GiftCardUsageHistoryID = @GiftCardUsageHistoryID
END
