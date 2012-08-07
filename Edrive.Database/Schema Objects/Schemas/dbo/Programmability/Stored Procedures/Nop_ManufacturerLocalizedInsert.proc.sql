

CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedInsert]
(
	@ManufacturerLocalizedID int = NULL output,
	@ManufacturerID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_ManufacturerLocalized]
	(
		ManufacturerID,
		LanguageID,
		[Name],
		[Description],		
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName
	)
	VALUES
	(
		@ManufacturerID,
		@LanguageID,
		@Name,
		@Description,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName
	)

	set @ManufacturerLocalizedID=@@identity

	EXEC Nop_ManufacturerLocalizedCleanUp
END
