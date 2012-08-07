

CREATE PROCEDURE [dbo].[Nop_PaymentMethod_RestrictedCountriesDelete]
(
	@PaymentMethodID int,
	@CountryID int
)
AS
BEGIN
	DELETE FROM 
		[Nop_PaymentMethod_RestrictedCountries]
	WHERE
		PaymentMethodID = @PaymentMethodID AND
		CountryID = @CountryID
END
