

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedUpdate]
(
	@SpecificationAttributeOptionLocalizedID int,
	@SpecificationAttributeOptionID int,
	@LanguageID int,
	@Name nvarchar(500)
)
AS
BEGIN
	
	UPDATE [Nop_SpecificationAttributeOptionLocalized]
	SET
		[SpecificationAttributeOptionID]=@SpecificationAttributeOptionID,
		[LanguageID]=@LanguageID,
		[Name]=@Name		
	WHERE
		SpecificationAttributeOptionLocalizedID = @SpecificationAttributeOptionLocalizedID

	EXEC Nop_SpecificationAttributeOptionLocalizedCleanUp
END
