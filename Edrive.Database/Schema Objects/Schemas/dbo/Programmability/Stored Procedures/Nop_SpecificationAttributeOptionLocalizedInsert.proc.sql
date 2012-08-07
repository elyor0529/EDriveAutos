

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedInsert]
(
	@SpecificationAttributeOptionLocalizedID int = NULL output,
	@SpecificationAttributeOptionID int,
	@LanguageID int,
	@Name nvarchar(500)
)
AS
BEGIN
	INSERT
	INTO [Nop_SpecificationAttributeOptionLocalized]
	(
		SpecificationAttributeOptionID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@SpecificationAttributeOptionID,
		@LanguageID,
		@Name
	)

	set @SpecificationAttributeOptionLocalizedID=@@identity

	EXEC Nop_SpecificationAttributeOptionLocalizedCleanUp
END
