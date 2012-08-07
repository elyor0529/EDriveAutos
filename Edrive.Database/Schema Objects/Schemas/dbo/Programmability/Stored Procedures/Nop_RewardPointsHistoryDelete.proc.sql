

CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryDelete]
(
	@RewardPointsHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_RewardPointsHistory]
	WHERE
		RewardPointsHistoryID = @RewardPointsHistoryID
END
