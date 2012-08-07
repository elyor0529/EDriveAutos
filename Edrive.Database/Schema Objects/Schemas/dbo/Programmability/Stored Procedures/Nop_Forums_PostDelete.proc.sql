

CREATE PROCEDURE [dbo].[Nop_Forums_PostDelete]
(
	@PostID int
)
AS
BEGIN
	SET NOCOUNT ON

	declare @UserID int
	declare @ForumID int
	declare @TopicID int
	SELECT 
		@UserID = ft.UserID,
		@ForumID = ft.ForumID,
		@TopicID = ft.TopicID
	FROM
		[Nop_Forums_Topic] ft
		INNER JOIN 
		[Nop_Forums_Post] fp
		ON ft.TopicID=fp.TopicID
	WHERE
		fp.PostID = @PostID 
	
	DELETE
	FROM [Nop_Forums_Post]
	WHERE
		PostID = @PostID

	--update stats/info
	exec [Nop_Forums_TopicUpdateCounts] @TopicID
	exec [Nop_Forums_ForumUpdateCounts] @ForumID
	exec [Nop_CustomerUpdateCounts] @UserID
END
