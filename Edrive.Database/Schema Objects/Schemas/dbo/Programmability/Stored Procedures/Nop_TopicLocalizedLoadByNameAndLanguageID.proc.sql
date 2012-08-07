

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadByNameAndLanguageID]
(
	@Name nvarchar(200),
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		tl.*
	FROM [Nop_TopicLocalized] tl
	INNER JOIN [Nop_Topic] t
	ON tl.TopicID = t.TopicID
	WHERE tl.LanguageID=@LanguageID and t.[Name] = @Name
END
