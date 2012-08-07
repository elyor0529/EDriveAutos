

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueDelete]
(
	@ProductVariantAttributeValueID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductVariantAttributeValue]
	WHERE
		ProductVariantAttributeValueID = @ProductVariantAttributeValueID
END
