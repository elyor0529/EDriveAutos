

CREATE PROCEDURE [dbo].[Nop_ProductLocalizedUpdate]
(
	@ProductLocalizedID int,
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
	
	UPDATE [Nop_ProductLocalized]
	SET
		[ProductID]=@ProductID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[ShortDescription]=@ShortDescription,
		[FullDescription]=@FullDescription,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName		
	WHERE
		ProductLocalizedID = @ProductLocalizedID

	EXEC Nop_ProductLocalizedCleanUp
END
