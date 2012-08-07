

CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingDelete]
(
	@ProductVariantAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductVariant_ProductAttribute_Mapping]
	WHERE
		ProductVariantAttributeID = @ProductVariantAttributeID
END
