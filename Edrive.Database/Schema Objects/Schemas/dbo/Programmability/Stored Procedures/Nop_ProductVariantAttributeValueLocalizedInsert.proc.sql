

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedInsert]
(
	@ProductVariantAttributeValueLocalizedID int = NULL output,
	@ProductVariantAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariantAttributeValueLocalized]
	(
		ProductVariantAttributeValueID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@ProductVariantAttributeValueID,
		@LanguageID,
		@Name
	)

	set @ProductVariantAttributeValueLocalizedID=@@identity

	EXEC Nop_ProductVariantAttributeValueLocalizedCleanUp
END
