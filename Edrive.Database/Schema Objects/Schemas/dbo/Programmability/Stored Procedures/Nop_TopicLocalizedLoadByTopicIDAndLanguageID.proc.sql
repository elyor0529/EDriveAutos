

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadByTopicIDAndLanguageID]
(
	@TopicID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TopicLocalized]
	WHERE (TopicID = @TopicID) AND (LanguageID = @LanguageID)
END
