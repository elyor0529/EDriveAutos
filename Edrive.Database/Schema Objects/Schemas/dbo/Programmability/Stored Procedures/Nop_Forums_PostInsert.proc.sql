

CREATE PROCEDURE [dbo].[Nop_Forums_PostInsert]
(
	@PostID int = NULL output,
	@TopicID int,
	@UserID int,
	@Text nvarchar(max),
	@IPAddress nvarchar(100),
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Post]
	(
		[TopicID],
		[UserID],
		[Text],
		[IPAddress],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@TopicID,
		@UserID,
		@Text,
		@IPAddress,
		@CreatedOn,
		@UpdatedOn
	)

	set @PostID=SCOPE_IDENTITY()

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
