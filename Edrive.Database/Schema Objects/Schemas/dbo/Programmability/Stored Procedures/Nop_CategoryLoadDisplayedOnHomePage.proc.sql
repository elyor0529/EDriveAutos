

CREATE PROCEDURE [dbo].[Nop_CategoryLoadDisplayedOnHomePage]
(
	@ShowHidden		bit = 0,
	@LanguageID		int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		c.CategoryID, 
		dbo.NOP_getnotnullnotempty(cl.Name,c.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cl.Description,c.Description) as [Description],
		c.TemplateID, 
		dbo.NOP_getnotnullnotempty(cl.MetaKeywords,c.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(cl.MetaDescription,c.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(cl.MetaTitle,c.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(cl.SEName,c.SEName) as [SEName],
		c.ParentCategoryID, 
		c.PictureID, 
		c.PageSize, 
		c.PriceRanges,
		c.ShowOnHomePage, 
		c.Published,
		c.Deleted, 
		c.DisplayOrder, 
		c.CreatedOn, 
		c.UpdatedOn
	FROM [Nop_Category] c
		LEFT OUTER JOIN [Nop_CategoryLocalized] cl 
		ON c.CategoryID = cl.CategoryID AND cl.LanguageID = @LanguageID	
	WHERE 
		(c.Published = 1 or @ShowHidden = 1) AND 
		c.Deleted=0 AND 
		c.ShowOnHomePage=1
	order by c.DisplayOrder
END
