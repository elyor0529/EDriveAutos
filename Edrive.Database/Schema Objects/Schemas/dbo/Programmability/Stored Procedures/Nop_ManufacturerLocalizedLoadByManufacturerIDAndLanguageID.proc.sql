


CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedLoadByManufacturerIDAndLanguageID]
	@ManufacturerID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ManufacturerLocalized]
	WHERE ManufacturerID = @ManufacturerID AND LanguageID=@LanguageID
	ORDER BY ManufacturerLocalizedID
END
