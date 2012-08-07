

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedUpdate]
(
	@SpecificationAttributeLocalizedID int,
	@SpecificationAttributeID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_SpecificationAttributeLocalized]
	SET
		[SpecificationAttributeID]=@SpecificationAttributeID,
		[LanguageID]=@LanguageID,
		[Name]=@Name		
	WHERE
		SpecificationAttributeLocalizedID = @SpecificationAttributeLocalizedID

	EXEC Nop_SpecificationAttributeLocalizedCleanUp
END
