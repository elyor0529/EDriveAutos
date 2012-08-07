

CREATE PROCEDURE [dbo].[Nop_Forums_ForumInsert]
(
	@ForumID int = NULL output,	
	@ForumGroupID int,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@NumTopics int,
	@NumPosts int,
	@LastTopicID int,
	@LastPostID int,
	@LastPostUserID int,
	@LastPostTime datetime,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Forum]
	(
		[ForumGroupID],
		[Name],
		[Description],
		[NumTopics],
		[NumPosts],
		[LastTopicID],
		[LastPostID],
		[LastPostUserID],
		[LastPostTime],
		[DisplayOrder],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@ForumGroupID,
		@Name,
		@Description,
		@NumTopics,
		@NumPosts,
		@LastTopicID,
		@LastPostID,
		@LastPostUserID,
		@LastPostTime,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @ForumID=SCOPE_IDENTITY()
END
