

CREATE PROCEDURE [dbo].[Nop_Forums_ForumDelete]
(
	@ForumID int
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE
	FROM [Nop_Forums_Subscription]	
	WHERE
		TopicID in (	SELECT ft.TopicID 
						FROM [Nop_Forums_Topic] ft
						WHERE ft.ForumID=@ForumID)

	DELETE
	FROM [Nop_Forums_Subscription]
	WHERE
		ForumID = @ForumID

	DELETE
	FROM [Nop_Forums_Forum]
	WHERE
		ForumID = @ForumID
	
END
