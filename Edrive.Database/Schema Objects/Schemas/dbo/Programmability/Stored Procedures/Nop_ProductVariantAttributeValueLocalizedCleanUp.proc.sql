

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductVariantAttributeValueLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
