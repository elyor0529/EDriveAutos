

CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadByGUID]
(
	@SubscriptionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Subscription]
	WHERE
		(SubscriptionGUID = @SubscriptionGUID)
END
