

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedUpdate]
(
	@ProductVariantAttributeValueLocalizedID int,
	@ProductVariantAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_ProductVariantAttributeValueLocalized]
	SET
		[ProductVariantAttributeValueID]=@ProductVariantAttributeValueID,
		[LanguageID]=@LanguageID,
		[Name]=@Name	
	WHERE
		ProductVariantAttributeValueLocalizedID = @ProductVariantAttributeValueLocalizedID

	EXEC Nop_ProductVariantAttributeValueLocalizedCleanUp
END
