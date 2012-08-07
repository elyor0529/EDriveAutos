

CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedInsert]
(
	@ProductAttributeLocalizedID int = NULL output,
	@ProductAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@Description nvarchar(400)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductAttributeLocalized]
	(
		ProductAttributeID,
		LanguageID,
		[Name],
		[Description]
	)
	VALUES
	(
		@ProductAttributeID,
		@LanguageID,
		@Name,
		@Description
	)

	set @ProductAttributeLocalizedID=@@identity

	EXEC Nop_ProductAttributeLocalizedCleanUp
END
