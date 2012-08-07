

CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadByPrimaryKey]
(
	@SubscriptionID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Subscription]
	WHERE
		(SubscriptionID = @SubscriptionID)
END
