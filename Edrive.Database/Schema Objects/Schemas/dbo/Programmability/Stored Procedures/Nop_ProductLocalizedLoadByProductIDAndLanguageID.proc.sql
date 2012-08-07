

CREATE PROCEDURE [dbo].[Nop_ProductLocalizedLoadByProductIDAndLanguageID]
	@ProductID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductLocalized]
	WHERE ProductID = @ProductID AND LanguageID=@LanguageID
	ORDER BY ProductLocalizedID
END
