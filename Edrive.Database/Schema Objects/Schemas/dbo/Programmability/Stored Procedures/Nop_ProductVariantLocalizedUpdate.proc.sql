

CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedUpdate]
(
	@ProductVariantLocalizedID int,
	@ProductVariantID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max)
)
AS
BEGIN
	
	UPDATE [Nop_ProductVariantLocalized]
	SET
		[ProductVariantID]=@ProductVariantID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description	
	WHERE
		ProductVariantLocalizedID = @ProductVariantLocalizedID

	EXEC Nop_ProductVariantLocalizedCleanUp
END
