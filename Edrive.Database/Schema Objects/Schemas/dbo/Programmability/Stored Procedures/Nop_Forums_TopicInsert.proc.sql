

CREATE PROCEDURE [dbo].[Nop_Forums_TopicInsert]
(
	@TopicID int = NULL output,
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
	INSERT
	INTO [Nop_Forums_Topic]
	(
		[ForumID],
		[UserID],
		[TopicTypeID],
		[Subject],
		[NumPosts],
		[Views],
		[LastPostID],
		[LastPostUserID],
		[LastPostTime],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@ForumID,
		@UserID,
		@TopicTypeID,
		@Subject,
		@NumPosts,
		@Views,
		@LastPostID,
		@LastPostUserID,
		@LastPostTime,
		@CreatedOn,
		@UpdatedOn
	)

	set @TopicID=SCOPE_IDENTITY()

	--update stats/info
	exec [Nop_Forums_ForumUpdateCounts] @ForumID
END
