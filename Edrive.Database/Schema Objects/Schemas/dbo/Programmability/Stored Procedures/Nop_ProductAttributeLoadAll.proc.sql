

CREATE PROCEDURE [dbo].[Nop_ProductAttributeLoadAll]
(
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		pa.ProductAttributeID, 
		dbo.NOP_getnotnullnotempty(pal.Name,pa.Name) as [Name], 
		dbo.NOP_getnotnullnotempty(pal.Description,pa.Description) as [Description]
	FROM [Nop_ProductAttribute] pa
		LEFT OUTER JOIN [Nop_ProductAttributeLocalized] pal
		ON pa.ProductAttributeID = pal.ProductAttributeID AND pal.LanguageID = @LanguageID	
	order by pa.[Name]
END
