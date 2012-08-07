

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedLoadBySpecificationAttributeIDAndLanguageID]
	@SpecificationAttributeID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeLocalized]
	WHERE SpecificationAttributeID = @SpecificationAttributeID AND LanguageID=@LanguageID
	ORDER BY SpecificationAttributeLocalizedID
END
