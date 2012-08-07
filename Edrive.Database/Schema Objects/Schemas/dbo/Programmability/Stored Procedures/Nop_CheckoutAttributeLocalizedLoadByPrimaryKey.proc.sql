

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedLoadByPrimaryKey]
	@CheckoutAttributeLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeLocalized]
	WHERE CheckoutAttributeLocalizedID = @CheckoutAttributeLocalizedID
	ORDER BY CheckoutAttributeLocalizedID
END
