

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByPrimaryKey]
	@ProductVariantAttributeValueLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantAttributeValueLocalized]
	WHERE ProductVariantAttributeValueLocalizedID = @ProductVariantAttributeValueLocalizedID
	ORDER BY ProductVariantAttributeValueLocalizedID
END
