

CREATE PROCEDURE [dbo].[Nop_ProductLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([ShortDescription] IS NULL OR [ShortDescription] = '') AND
		([FullDescription] IS NULL OR [FullDescription] = '') AND
		(MetaKeywords IS NULL or MetaKeywords = '') AND
		(MetaDescription IS NULL or MetaDescription = '') AND
		(MetaTitle IS NULL or MetaTitle = '') AND
		(SEName IS NULL or SEName = '') 
END
