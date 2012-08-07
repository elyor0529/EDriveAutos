

CREATE PROCEDURE [dbo].[Nop_ProductLocalizedInsert]
(
	@ProductLocalizedID int = NULL output,
	@ProductID int,
	@LanguageID int,
	@Name nvarchar(400),
	@ShortDescription nvarchar(max),
	@FullDescription nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductLocalized]
	(
		ProductID,
		LanguageID,
		[Name],
		[ShortDescription],
		[FullDescription],	
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName
	)
	VALUES
	(
		@ProductID,
		@LanguageID,
		@Name,
		@ShortDescription,
		@FullDescription,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName
	)

	set @ProductLocalizedID=@@identity

	EXEC Nop_ProductLocalizedCleanUp
END
