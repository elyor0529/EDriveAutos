

CREATE PROCEDURE [dbo].[Nop_ShippingMethod_RestrictedCountriesDelete]
(
	@ShippingMethodID int,
	@CountryID int
)
AS
BEGIN
	DELETE FROM 
		[Nop_ShippingMethod_RestrictedCountries]
	WHERE
		ShippingMethodID = @ShippingMethodID AND
		CountryID = @CountryID
END
