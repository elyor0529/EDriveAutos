

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_SpecificationAttributeOptionLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
