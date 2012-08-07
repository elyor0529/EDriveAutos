

CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeCombinationLoadAll]	
	@ProductVariantID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ProductVariantAttributeCombination]
	WHERE
		ProductVariantID = @ProductVariantID
	order by [ProductVariantAttributeCombinationID]
END
