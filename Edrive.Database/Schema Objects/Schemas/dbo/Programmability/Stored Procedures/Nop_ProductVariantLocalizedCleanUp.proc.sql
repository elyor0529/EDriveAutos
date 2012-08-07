

CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductVariantLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '')
END
