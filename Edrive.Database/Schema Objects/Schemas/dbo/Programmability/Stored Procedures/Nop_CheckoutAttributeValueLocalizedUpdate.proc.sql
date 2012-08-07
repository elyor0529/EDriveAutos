

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedUpdate]
(
	@CheckoutAttributeValueLocalizedID int,
	@CheckoutAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_CheckoutAttributeValueLocalized]
	SET
		[CheckoutAttributeValueID]=@CheckoutAttributeValueID,
		[LanguageID]=@LanguageID,
		[Name]=@Name	
	WHERE
		CheckoutAttributeValueLocalizedID = @CheckoutAttributeValueLocalizedID

	EXEC Nop_CheckoutAttributeValueLocalizedCleanUp
END
