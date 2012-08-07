

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByProductVariantAttributeValueIDAndLanguageID]
	@ProductVariantAttributeValueID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantAttributeValueLocalized]
	WHERE ProductVariantAttributeValueID = @ProductVariantAttributeValueID AND LanguageID=@LanguageID
	ORDER BY ProductVariantAttributeValueLocalizedID
END
