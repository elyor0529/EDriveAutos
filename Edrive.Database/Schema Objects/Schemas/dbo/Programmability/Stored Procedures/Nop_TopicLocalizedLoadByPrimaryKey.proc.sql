

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadByPrimaryKey]
(
	@TopicLocalizedID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TopicLocalized]
	WHERE
		TopicLocalizedID = @TopicLocalizedID
END
