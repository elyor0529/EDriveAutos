

CREATE PROCEDURE [dbo].[Nop_LocaleStringResourceLoadAllByLanguageID]
(
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_LocaleStringResource]
	WHERE
		(LanguageID = @LanguageID)
	ORDER BY ResourceName
END
