

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightAndCountryUpdate]
(
	@ShippingByWeightAndCountryID int,
	@ShippingMethodID int,
	@CountryID int,
	@From decimal(18, 2),
	@To decimal(18, 2),
	@UsePercentage bit,
	@ShippingChargePercentage decimal(18, 2),
	@ShippingChargeAmount decimal(18, 2)
)
AS
BEGIN
	UPDATE [Nop_ShippingByWeightAndCountry]
	SET
		ShippingMethodID=@ShippingMethodID,
		CountryID=@CountryID,
		[From]=@From,
		[To]=@To,
		UsePercentage=@UsePercentage,
		ShippingChargePercentage=@ShippingChargePercentage,
		ShippingChargeAmount=@ShippingChargeAmount
	WHERE
		ShippingByWeightAndCountryID = @ShippingByWeightAndCountryID
END
