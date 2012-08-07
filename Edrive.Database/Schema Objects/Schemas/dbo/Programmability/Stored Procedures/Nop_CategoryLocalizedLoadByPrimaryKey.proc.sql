

CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedLoadByPrimaryKey]
	@CategoryLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CategoryLocalized]
	WHERE CategoryLocalizedID = @CategoryLocalizedID
	ORDER BY CategoryLocalizedID
END
