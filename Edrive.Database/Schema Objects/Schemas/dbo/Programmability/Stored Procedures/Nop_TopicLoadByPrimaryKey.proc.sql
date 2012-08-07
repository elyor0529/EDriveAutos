

CREATE PROCEDURE [dbo].[Nop_TopicLoadByPrimaryKey]
(
	@TopicID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Topic]
	WHERE
		TopicID = @TopicID
END
