

CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedInsert]
(
	@CategoryLocalizedID int = NULL output,
	@CategoryID int,
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
	INTO [Nop_CategoryLocalized]
	(
		CategoryID,
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
		@CategoryID,
		@LanguageID,
		@Name,
		@Description,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName
	)

	set @CategoryLocalizedID=@@identity

	EXEC Nop_CategoryLocalizedCleanUp
END
