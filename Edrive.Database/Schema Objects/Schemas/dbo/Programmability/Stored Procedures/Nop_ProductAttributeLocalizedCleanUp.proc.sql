

CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductAttributeLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '')
END
