

CREATE PROCEDURE [dbo].[Nop_GiftCardUsageHistoryDelete]
(
	@GiftCardUsageHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_GiftCardUsageHistory]
	WHERE
		GiftCardUsageHistoryID = @GiftCardUsageHistoryID
END
