

CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingLoadByProductVariantID]
(
	@ProductVariantID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ProductVariant_ProductAttribute_Mapping]
	WHERE ProductVariantID=@ProductVariantID
	ORDER BY [DisplayOrder]
END
