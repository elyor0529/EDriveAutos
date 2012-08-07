

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedLoadByCheckoutAttributeIDAndLanguageID]
	@CheckoutAttributeID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeLocalized]
	WHERE CheckoutAttributeID = @CheckoutAttributeID AND LanguageID=@LanguageID
	ORDER BY CheckoutAttributeLocalizedID
END
