

CREATE PROCEDURE [dbo].[Nop_ShippingMethod_RestrictedCountriesInsert]
(
	@ShippingMethodID int,
	@CountryID int
)
AS
BEGIN
	IF(NOT EXISTS(SELECT * FROM [Nop_ShippingMethod_RestrictedCountries] WHERE ShippingMethodID = @ShippingMethodID AND CountryID = @CountryID))
	BEGIN
		INSERT
		INTO [Nop_ShippingMethod_RestrictedCountries]
		(
			ShippingMethodID,
			CountryID
		)
		VALUES
		(
			@ShippingMethodID,
			@CountryID
		)
	END
END
