

CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingLoadByPrimaryKey]
(
	@ProductVariantAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductVariant_ProductAttribute_Mapping]
	WHERE
		ProductVariantAttributeID = @ProductVariantAttributeID
END
