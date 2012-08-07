

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedInsert]
(
	@SpecificationAttributeLocalizedID int = NULL output,
	@SpecificationAttributeID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_SpecificationAttributeLocalized]
	(
		SpecificationAttributeID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@SpecificationAttributeID,
		@LanguageID,
		@Name
	)

	set @SpecificationAttributeLocalizedID=@@identity

	EXEC Nop_SpecificationAttributeLocalizedCleanUp
END
