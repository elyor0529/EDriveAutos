

CREATE PROCEDURE [dbo].[Nop_Forums_ForumUpdate]
(
	@ForumID int,	
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
	UPDATE [Nop_Forums_Forum]
	SET
		ForumGroupID=@ForumGroupID,
		[Name]=@Name,
		[Description]=@Description,
		NumTopics=@NumTopics,
		NumPosts=@NumPosts,
		LastTopicID=@LastTopicID,
		LastPostID=@LastPostID,
		LastPostUserID=@LastPostUserID,
		LastPostTime=@LastPostTime,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ForumID = @ForumID
END
