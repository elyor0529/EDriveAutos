

CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadAllByName]
(
	@Name nvarchar(200)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT tl.*
	FROM [Nop_TopicLocalized] tl
	INNER JOIN [Nop_Topic] t
	ON tl.TopicID = t.TopicID
	WHERE t.[Name] = @Name
	ORDER BY tl.LanguageID
END
