

CREATE PROCEDURE [dbo].[Nop_CategoryUpdate]
(
	@CategoryID int,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@ParentCategoryID int,
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@ShowOnHomePage bit,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Category]
	SET
		[Name]=@Name,
		[Description]=@Description,
		TemplateID=@TemplateID,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName,
		ParentCategoryID=@ParentCategoryID,
		PictureID=@PictureID,
		PageSize=@PageSize,
		PriceRanges=@PriceRanges,
		ShowOnHomePage=@ShowOnHomePage,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		CategoryID = @CategoryID
END
