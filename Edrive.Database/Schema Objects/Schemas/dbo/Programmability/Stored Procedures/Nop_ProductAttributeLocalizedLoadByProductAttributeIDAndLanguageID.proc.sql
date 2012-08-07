

CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedLoadByProductAttributeIDAndLanguageID]
	@ProductAttributeID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductAttributeLocalized]
	WHERE ProductAttributeID = @ProductAttributeID AND LanguageID=@LanguageID
	ORDER BY ProductAttributeLocalizedID
END
