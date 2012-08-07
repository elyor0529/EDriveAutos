

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedLoadByPrimaryKey]
	@CheckoutAttributeValueLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeValueLocalized]
	WHERE CheckoutAttributeValueLocalizedID = @CheckoutAttributeValueLocalizedID
	ORDER BY CheckoutAttributeValueLocalizedID
END
