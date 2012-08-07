

CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadByPrimaryKey]
(
	@TopicID int,
	@IncreaseViews bit = 0
)
AS
BEGIN
	SET NOCOUNT ON

	IF (@IncreaseViews = 1)
	BEGIN
		UPDATE [Nop_Forums_Topic]
		SET 
			[Views]=[Views]+1
		WHERE
			TopicID = @TopicID 
	END

	SELECT
		*
	FROM [Nop_Forums_Topic]
	WHERE
		(TopicID = @TopicID)
END
