

CREATE PROCEDURE [dbo].[Nop_Forums_PostUpdate]
(
	@PostID int,
	@TopicID int,
	@UserID int,
	@Text nvarchar(max),
	@IPAddress nvarchar(100),
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Post]
	SET
		[TopicID]=@TopicID,
		[UserID]=@UserID,
		[Text]=@Text,
		[IPAddress]=@IPAddress,
		[CreatedOn]=@CreatedOn,
		[UpdatedOn]=@UpdatedOn
	WHERE
		PostID = @PostID

	--update stats/info
	exec [Nop_Forums_TopicUpdateCounts] @TopicID
	
	declare @ForumID int
	SELECT 
		@ForumID = ft.ForumID
	FROM
		[Nop_Forums_Topic] ft
	WHERE
		ft.TopicID = @TopicID 
		
	exec [Nop_Forums_ForumUpdateCounts] @ForumID
	
	exec [Nop_CustomerUpdateCounts] @UserID
END
