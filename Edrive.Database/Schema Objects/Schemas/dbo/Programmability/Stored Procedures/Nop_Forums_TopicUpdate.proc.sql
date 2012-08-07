

CREATE PROCEDURE [dbo].[Nop_Forums_TopicUpdate]
(
	@TopicID int,
	@ForumID int,
	@UserID int,
	@TopicTypeID int,
	@Subject nvarchar(450),
	@NumPosts int,
	@Views int,
	@LastPostID int,
	@LastPostUserID int,
	@LastPostTime datetime,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Topic]
	SET
		[ForumID]=@ForumID,
		[UserID]=@UserID,
		[TopicTypeID]=@TopicTypeID,
		[Subject]=@Subject,
		[NumPosts]=@NumPosts,
		[Views]=@Views,
		LastPostID=@LastPostID,
		LastPostUserID=@LastPostUserID,
		LastPostTime=@LastPostTime,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		TopicID = @TopicID
	
	--update stats/info
	exec [Nop_Forums_ForumUpdateCounts] @ForumID
END
