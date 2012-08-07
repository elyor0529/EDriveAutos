

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightAndCountryLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ShippingByWeightAndCountry]
	ORDER BY CountryID, ShippingMethodID, [From]
END
