

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedUpdate]
(
	@CheckoutAttributeLocalizedID int,
	@CheckoutAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300)
)
AS
BEGIN
	
	UPDATE [Nop_CheckoutAttributeLocalized]
	SET
		[CheckoutAttributeID]=@CheckoutAttributeID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[TextPrompt]=@TextPrompt
	WHERE
		CheckoutAttributeLocalizedID = @CheckoutAttributeLocalizedID

	EXEC Nop_CheckoutAttributeLocalizedCleanUp
END
