

CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedLoadByProductVariantIDAndLanguageID]
	@ProductVariantID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantLocalized]
	WHERE ProductVariantID = @ProductVariantID AND LanguageID=@LanguageID
	ORDER BY ProductVariantLocalizedID
END
