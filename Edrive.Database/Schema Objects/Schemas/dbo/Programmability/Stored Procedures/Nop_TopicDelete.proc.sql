

CREATE PROCEDURE [dbo].[Nop_TopicDelete]
(
	@TopicID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Topic]
	WHERE
		[TopicID] = @TopicID
END
