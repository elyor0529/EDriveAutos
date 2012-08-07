

CREATE PROCEDURE [dbo].[Nop_PaymentMethod_RestrictedCountriesContains]
(
	@PaymentMethodID int,
	@CountryID int,
	@Result bit output
)
AS
BEGIN
	IF(NOT EXISTS(SELECT * FROM [Nop_PaymentMethod_RestrictedCountries]WHERE PaymentMethodID = @PaymentMethodID AND CountryID = @CountryID))
		SET @Result = 0
	ELSE
		SET @Result = 1
END
