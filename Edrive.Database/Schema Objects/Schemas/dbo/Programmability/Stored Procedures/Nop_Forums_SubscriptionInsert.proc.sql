

CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionInsert]
(
	@SubscriptionID int = NULL output,
	@SubscriptionGUID uniqueidentifier,
	@UserID int,
	@ForumID int,
	@TopicID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Subscription]
	(
		[SubscriptionGUID],
		[UserID],
		[ForumID],
		[TopicID],
		[CreatedOn]
	)
	VALUES
	(
		@SubscriptionGUID,
		@UserID,
		@ForumID,
		@TopicID,
		@CreatedOn
	)

	set @SubscriptionID=SCOPE_IDENTITY()
END
