

CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedLoadByCategoryIDAndLanguageID]
	@CategoryID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CategoryLocalized]
	WHERE CategoryID = @CategoryID AND LanguageID=@LanguageID
	ORDER BY CategoryLocalizedID
END
