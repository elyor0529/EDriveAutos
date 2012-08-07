

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeCombinationDelete]
(
	@ProductVariantAttributeCombinationID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductVariantAttributeCombination]
	WHERE
		ProductVariantAttributeCombinationID = @ProductVariantAttributeCombinationID
END
