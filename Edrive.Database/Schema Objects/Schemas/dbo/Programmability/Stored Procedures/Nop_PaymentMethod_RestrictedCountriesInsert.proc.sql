

CREATE PROCEDURE [dbo].[Nop_PaymentMethod_RestrictedCountriesInsert]
(
	@PaymentMethodID int,
	@CountryID int
)
AS
BEGIN
	IF(NOT EXISTS(SELECT * FROM [Nop_PaymentMethod_RestrictedCountries] WHERE PaymentMethodID = @PaymentMethodID AND CountryID = @CountryID))
	BEGIN
		INSERT
		INTO [Nop_PaymentMethod_RestrictedCountries]
		(
			PaymentMethodID,
			CountryID
		)
		VALUES
		(
			@PaymentMethodID,
			@CountryID
		)
	END
END
