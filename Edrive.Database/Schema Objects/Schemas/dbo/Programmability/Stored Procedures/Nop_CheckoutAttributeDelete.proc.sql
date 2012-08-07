

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeDelete]
(
	@CheckoutAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CheckoutAttribute]
	WHERE
		CheckoutAttributeID = @CheckoutAttributeID
END
