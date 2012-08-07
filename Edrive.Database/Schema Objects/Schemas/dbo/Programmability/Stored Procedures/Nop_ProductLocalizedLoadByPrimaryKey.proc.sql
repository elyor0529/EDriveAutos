

CREATE PROCEDURE [dbo].[Nop_ProductLocalizedLoadByPrimaryKey]
	@ProductLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductLocalized]
	WHERE ProductLocalizedID = @ProductLocalizedID
	ORDER BY ProductLocalizedID
END
