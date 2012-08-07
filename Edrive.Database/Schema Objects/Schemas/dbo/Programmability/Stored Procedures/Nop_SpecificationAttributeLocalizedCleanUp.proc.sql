

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_SpecificationAttributeLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
