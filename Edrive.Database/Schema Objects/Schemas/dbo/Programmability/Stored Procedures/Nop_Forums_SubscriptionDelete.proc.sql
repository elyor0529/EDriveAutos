

CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionDelete]
(
	@SubscriptionID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Forums_Subscription]
	WHERE
		SubscriptionID = @SubscriptionID
END
