

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedLoadByCheckoutAttributeValueIDAndLanguageID]
	@CheckoutAttributeValueID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeValueLocalized]
	WHERE CheckoutAttributeValueID = @CheckoutAttributeValueID AND LanguageID=@LanguageID
	ORDER BY CheckoutAttributeValueLocalizedID
END
