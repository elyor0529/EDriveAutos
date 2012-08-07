

CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedUpdate]
(
	@ProductAttributeLocalizedID int,
	@ProductAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@Description nvarchar(400)
)
AS
BEGIN
	
	UPDATE [Nop_ProductAttributeLocalized]
	SET
		[ProductAttributeID]=@ProductAttributeID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description
	WHERE
		ProductAttributeLocalizedID = @ProductAttributeLocalizedID

	EXEC Nop_ProductAttributeLocalizedCleanUp
END
