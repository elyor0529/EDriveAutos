

CREATE PROCEDURE [dbo].[Nop_StateProvinceLoadAllByCountryID]
	@CountryID int
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT *
	FROM [Nop_StateProvince]
	WHERE CountryID=@CountryID 
	ORDER BY DisplayOrder
END
