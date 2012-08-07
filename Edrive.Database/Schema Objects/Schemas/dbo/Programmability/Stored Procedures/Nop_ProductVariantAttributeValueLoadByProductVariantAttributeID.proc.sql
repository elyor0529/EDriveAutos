

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLoadByProductVariantAttributeID]
(
	@ProductVariantAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pvav.ProductVariantAttributeValueID, 
		pvav.ProductVariantAttributeID, 
		dbo.NOP_getnotnullnotempty(pvavl.Name,pvav.Name) as [Name],
		pvav.PriceAdjustment, 
		pvav.WeightAdjustment, 
		pvav.IsPreSelected, 
		pvav.DisplayOrder
	FROM [Nop_ProductVariantAttributeValue] pvav
		LEFT OUTER JOIN [Nop_ProductVariantAttributeValueLocalized] pvavl 
		ON pvav.ProductVariantAttributeValueID = pvavl.ProductVariantAttributeValueID AND pvavl.LanguageID = @LanguageID	
	WHERE 
		pvav.ProductVariantAttributeID=@ProductVariantAttributeID
	ORDER BY pvav.[DisplayOrder]
END
