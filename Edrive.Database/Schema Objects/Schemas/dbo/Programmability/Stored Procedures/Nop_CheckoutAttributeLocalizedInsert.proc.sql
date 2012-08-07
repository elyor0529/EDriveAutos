

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedInsert]
(
	@CheckoutAttributeLocalizedID int = NULL output,
	@CheckoutAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300)
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttributeLocalized]
	(
		CheckoutAttributeID,
		LanguageID,
		[Name],
		[TextPrompt]
	)
	VALUES
	(
		@CheckoutAttributeID,
		@LanguageID,
		@Name,
		@TextPrompt
	)

	set @CheckoutAttributeLocalizedID=@@identity

	EXEC Nop_CheckoutAttributeLocalizedCleanUp
END
