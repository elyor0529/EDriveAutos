

CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionUpdate]
(
	@SubscriptionID int,
	@SubscriptionGUID uniqueidentifier,
	@UserID int,
	@ForumID int,
	@TopicID int,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Subscription]
	SET
		[SubscriptionGUID]=@SubscriptionGUID,
		[UserID]=@UserID,
		[ForumID]=@ForumID,
		[TopicID]=@TopicID,
		[CreatedOn]=@CreatedOn
	WHERE
		SubscriptionID = @SubscriptionID
END
