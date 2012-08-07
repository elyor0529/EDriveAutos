

CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadActive]
(
	@ForumID			int,
	@TopicCount			int
)
AS
BEGIN
	if (@TopicCount > 0)
	      SET ROWCOUNT @TopicCount

	SELECT ft2.* FROM Nop_Forums_Topic ft2 with (NOLOCK) 
	WHERE ft2.TopicID IN 
	(
		SELECT DISTINCT
			ft.TopicID
		FROM Nop_Forums_Topic ft with (NOLOCK)
		WHERE  (
					@ForumID IS NULL OR @ForumID=0
					OR (ft.ForumID=@ForumID)
				)
				AND
				(
					ft.LastPostTime is not null
				)
	)
	ORDER BY ft2.LastPostTime desc

	SET ROWCOUNT 0
END
