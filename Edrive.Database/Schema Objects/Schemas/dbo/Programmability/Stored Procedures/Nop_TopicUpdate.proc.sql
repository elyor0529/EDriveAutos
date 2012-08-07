

CREATE PROCEDURE [dbo].[Nop_TopicUpdate]
(
	@TopicID int,
	@Name nvarchar(200)
)
AS
BEGIN
	UPDATE [Nop_Topic]
	SET
		[Name]=@Name
	WHERE
		TopicID = @TopicID
END
