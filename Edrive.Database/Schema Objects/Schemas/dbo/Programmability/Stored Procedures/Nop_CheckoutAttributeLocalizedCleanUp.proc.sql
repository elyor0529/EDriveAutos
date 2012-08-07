

CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_CheckoutAttributeLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([TextPrompt] IS NULL OR [TextPrompt] = '')
END
