

CREATE PROCEDURE [dbo].[Nop_ShippingMethod_RestrictedCountriesContains]
(
	@ShippingMethodID int,
	@CountryID int,
	@Result bit output
)
AS
BEGIN
	IF(NOT EXISTS(SELECT * FROM [Nop_ShippingMethod_RestrictedCountries] WHERE ShippingMethodID = @ShippingMethodID AND CountryID = @CountryID))
		SET @Result = 0
	ELSE
		SET @Result = 1
END
