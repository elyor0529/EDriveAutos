

CREATE PROCEDURE [dbo].[Nop_Forums_TopicDelete]
(
	@TopicID int
)
AS
BEGIN
	SET NOCOUNT ON

	declare @UserID int
	declare @ForumID int
	SELECT 
		@UserID = UserID,
		@ForumID = ForumID
	FROM
		[Nop_Forums_Topic]
	WHERE
		TopicID = @TopicID 

	DELETE
	FROM [Nop_Forums_Topic]
	WHERE
		TopicID = @TopicID

	DELETE
	FROM [Nop_Forums_Subscription]
	WHERE
		TopicID = @TopicID

	--update stats/info
	exec [Nop_Forums_ForumUpdateCounts] @ForumID
	exec [Nop_CustomerUpdateCounts] @UserID
END
