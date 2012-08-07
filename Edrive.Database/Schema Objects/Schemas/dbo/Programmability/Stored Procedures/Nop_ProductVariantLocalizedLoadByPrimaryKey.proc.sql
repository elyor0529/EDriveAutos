

CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedLoadByPrimaryKey]
	@ProductVariantLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantLocalized]
	WHERE ProductVariantLocalizedID = @ProductVariantLocalizedID
	ORDER BY ProductVariantLocalizedID
END
