

CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryLoadByPrimaryKey]
(
	@RewardPointsHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_RewardPointsHistory]
	WHERE
		RewardPointsHistoryID = @RewardPointsHistoryID
END
