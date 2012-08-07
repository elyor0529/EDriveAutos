

CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedLoadByPrimaryKey]
	@ProductAttributeLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductAttributeLocalized]
	WHERE ProductAttributeLocalizedID = @ProductAttributeLocalizedID
	ORDER BY ProductAttributeLocalizedID
END
