

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedLoadBySpecificationAttributeOptionIDAndLanguageID]
	@SpecificationAttributeOptionID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeOptionLocalized]
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID AND LanguageID=@LanguageID
	ORDER BY SpecificationAttributeOptionLocalizedID
END
