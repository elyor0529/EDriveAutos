

CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedInsert]
(
	@ProductVariantLocalizedID int = NULL output,
	@ProductVariantID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariantLocalized]
	(
		ProductVariantID,
		LanguageID,
		[Name],
		[Description]
	)
	VALUES
	(
		@ProductVariantID,
		@LanguageID,
		@Name,
		@Description
	)

	set @ProductVariantLocalizedID=@@identity

	EXEC Nop_ProductVariantLocalizedCleanUp
END
