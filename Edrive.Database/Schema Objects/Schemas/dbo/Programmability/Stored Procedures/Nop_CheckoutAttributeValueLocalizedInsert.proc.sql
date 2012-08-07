

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedInsert]
(
	@CheckoutAttributeValueLocalizedID int = NULL output,
	@CheckoutAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttributeValueLocalized]
	(
		CheckoutAttributeValueID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@CheckoutAttributeValueID,
		@LanguageID,
		@Name
	)

	set @CheckoutAttributeValueLocalizedID=@@identity

	EXEC Nop_CheckoutAttributeValueLocalizedCleanUp
END
