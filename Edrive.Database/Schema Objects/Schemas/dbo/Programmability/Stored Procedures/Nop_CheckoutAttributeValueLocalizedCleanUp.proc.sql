


CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_CheckoutAttributeValueLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
