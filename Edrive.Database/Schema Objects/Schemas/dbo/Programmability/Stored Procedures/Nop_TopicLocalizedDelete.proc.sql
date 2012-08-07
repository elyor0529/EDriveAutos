

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedDelete]
(
	@TopicLocalizedID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TopicLocalized]
	WHERE
		TopicLocalizedID = @TopicLocalizedID
END
