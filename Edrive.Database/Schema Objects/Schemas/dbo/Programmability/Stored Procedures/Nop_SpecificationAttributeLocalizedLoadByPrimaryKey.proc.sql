

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedLoadByPrimaryKey]
	@SpecificationAttributeLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeLocalized]
	WHERE SpecificationAttributeLocalizedID = @SpecificationAttributeLocalizedID
	ORDER BY SpecificationAttributeLocalizedID
END
