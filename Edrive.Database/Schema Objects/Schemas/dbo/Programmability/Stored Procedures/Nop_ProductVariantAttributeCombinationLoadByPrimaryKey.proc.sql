

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeCombinationLoadByPrimaryKey]
(
	@ProductVariantAttributeCombinationID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductVariantAttributeCombination]
	WHERE
		ProductVariantAttributeCombinationID = @ProductVariantAttributeCombinationID
END
