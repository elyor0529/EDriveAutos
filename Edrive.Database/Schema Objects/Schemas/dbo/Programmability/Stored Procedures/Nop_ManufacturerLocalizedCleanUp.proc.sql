

CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ManufacturerLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '') AND
		(MetaKeywords IS NULL or MetaKeywords = '') AND
		(MetaDescription IS NULL or MetaDescription = '') AND
		(MetaTitle IS NULL or MetaTitle = '') AND
		(SEName IS NULL or SEName = '') 
END
