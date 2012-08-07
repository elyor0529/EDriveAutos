

CREATE PROCEDURE [dbo].[Nop_ManufacturerLoadAll]
	@ShowHidden bit = 0,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		m.ManufacturerID, 
		dbo.NOP_getnotnullnotempty(ml.Name,m.Name) as [Name],
		dbo.NOP_getnotnullnotempty(ml.Description,m.Description) as [Description],
		m.TemplateID, 
		dbo.NOP_getnotnullnotempty(ml.MetaKeywords,m.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(ml.MetaDescription,m.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(ml.MetaTitle,m.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(ml.SEName,m.SEName) as [SEName],
		m.PictureID, 
		m.PageSize, 
		m.PriceRanges, 
		m.Published,
		m.Deleted, 
		m.DisplayOrder, 
		m.CreatedOn, 
		m.UpdatedOn
	FROM [Nop_Manufacturer] m
		LEFT OUTER JOIN [Nop_ManufacturerLocalized] ml 
		ON m.ManufacturerID = ml.ManufacturerID AND ml.LanguageID = @LanguageID	
	WHERE 
		(m.Published = 1 or @ShowHidden = 1) AND 
		m.Deleted=0
	order by m.DisplayOrder
END
