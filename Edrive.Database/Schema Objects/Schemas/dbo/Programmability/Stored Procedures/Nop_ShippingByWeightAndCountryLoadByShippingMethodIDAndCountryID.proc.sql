

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightAndCountryLoadByShippingMethodIDAndCountryID]
(
	@ShippingMethodID int,
	@CountryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingByWeightAndCountry]
	WHERE
		ShippingMethodID=@ShippingMethodID and CountryID=@CountryID
	ORDER BY [From]
END
