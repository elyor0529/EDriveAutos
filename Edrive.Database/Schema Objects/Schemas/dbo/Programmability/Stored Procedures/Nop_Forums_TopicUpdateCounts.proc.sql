

CREATE PROCEDURE [dbo].[Nop_Forums_TopicUpdateCounts]
(
	@TopicID int
)
AS
BEGIN

	DECLARE @NumPosts int
	DECLARE @LastPostID int
	DECLARE @LastPostUserID int
	DECLARE @LastPostTime datetime

	SELECT 
		@NumPosts = COUNT(1)
	FROM
		[Nop_Forums_Post] fp
	WHERE
		fp.TopicID = @TopicID


	SELECT TOP 1
		@LastPostID = fp.PostID,
		@LastPostUserID = fp.UserID,
		@LastPostTime = fp.CreatedOn
	FROM [Nop_Forums_Post] fp
	WHERE
		fp.TopicID = @TopicID
	ORDER BY fp.CreatedOn desc

	SET @NumPosts = isnull(@NumPosts, 0)
	SET @LastPostID = isnull(@LastPostID, 0)
	SET @LastPostUserID = isnull(@LastPostUserID, 0)

	SET NOCOUNT ON
	UPDATE 
		[Nop_Forums_Topic]
	SET
		[NumPosts] = @NumPosts,
		[LastPostID] = @LastPostID,
		[LastPostUserID] = @LastPostUserID,
		[LastPostTime] = @LastPostTime
	WHERE
		TopicID = @TopicID
END
