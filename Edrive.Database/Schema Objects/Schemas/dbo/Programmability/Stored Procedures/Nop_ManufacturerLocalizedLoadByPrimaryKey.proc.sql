

CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedLoadByPrimaryKey]
	@ManufacturerLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ManufacturerLocalized]
	WHERE ManufacturerLocalizedID = @ManufacturerLocalizedID
	ORDER BY ManufacturerLocalizedID
END
