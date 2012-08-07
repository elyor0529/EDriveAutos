

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueDelete]
(
	@CheckoutAttributeValueID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CheckoutAttributeValue]
	WHERE
		CheckoutAttributeValueID = @CheckoutAttributeValueID
END
