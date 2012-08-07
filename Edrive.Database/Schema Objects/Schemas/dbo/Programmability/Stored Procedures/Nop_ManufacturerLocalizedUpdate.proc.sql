

CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedUpdate]
(
	@ManufacturerLocalizedID int,
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
	
	UPDATE [Nop_ManufacturerLocalized]
	SET
		[ManufacturerID]=@ManufacturerID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description,		
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName		
	WHERE
		ManufacturerLocalizedID = @ManufacturerLocalizedID

	EXEC Nop_ManufacturerLocalizedCleanUp
END
