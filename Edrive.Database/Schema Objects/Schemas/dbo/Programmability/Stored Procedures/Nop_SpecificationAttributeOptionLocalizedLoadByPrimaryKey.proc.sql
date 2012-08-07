
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedLoadByPrimaryKey]
	@SpecificationAttributeOptionLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeOptionLocalized]
	WHERE SpecificationAttributeOptionLocalizedID = @SpecificationAttributeOptionLocalizedID
	ORDER BY SpecificationAttributeOptionLocalizedID
END
