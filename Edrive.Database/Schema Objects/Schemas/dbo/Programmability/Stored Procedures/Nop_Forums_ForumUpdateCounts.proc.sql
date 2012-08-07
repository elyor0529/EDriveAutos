

CREATE PROCEDURE [dbo].[Nop_Forums_ForumUpdateCounts]
(
	@ForumID int
)
AS
BEGIN

	DECLARE @NumTopics int
	DECLARE @NumPosts int
	DECLARE @LastTopicID int
	DECLARE @LastPostID int
	DECLARE @LastPostUserID int
	DECLARE @LastPostTime datetime

	SELECT 
		@NumTopics =COUNT(1) 
	FROM [Nop_Forums_Topic]
	WHERE [ForumID] = @ForumID

	SELECT 
		@NumPosts = COUNT(1)
	FROM
		[Nop_Forums_Topic] ft
		INNER JOIN [Nop_Forums_Post] fp on ft.TopicID=fp.TopicID
	WHERE
		ft.ForumID = @ForumID


	SELECT TOP 1
		@LastTopicID = ft.TopicID,
		@LastPostID = fp.PostID,
		@LastPostUserID = fp.UserID,
		@LastPostTime = fp.CreatedOn
	FROM
		[Nop_Forums_Topic] ft
		LEFT OUTER JOIN [Nop_Forums_Post] fp on ft.TopicID=fp.TopicID
	WHERE
		ft.ForumID = @ForumID
	ORDER BY fp.CreatedOn desc, ft.CreatedOn desc

	SET @NumTopics = isnull(@NumTopics, 0)
	SET @NumPosts = isnull(@NumPosts, 0)
	SET @LastTopicID = isnull(@LastTopicID, 0)
	SET @LastPostID = isnull(@LastPostID, 0)
	SET @LastPostUserID = isnull(@LastPostUserID, 0)

	SET NOCOUNT ON
	UPDATE 
		[Nop_Forums_Forum]
	SET
		[NumTopics] = @NumTopics,
		[NumPosts] = @NumPosts,
		[LastTopicID] = @LastTopicID,
		[LastPostID] = @LastPostID,
		[LastPostUserID] = @LastPostUserID,
		[LastPostTime] = @LastPostTime
	WHERE
		ForumID = @ForumID
END
