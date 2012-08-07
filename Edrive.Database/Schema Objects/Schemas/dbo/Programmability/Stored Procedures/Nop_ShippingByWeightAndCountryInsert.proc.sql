

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightAndCountryInsert]
(
	@ShippingByWeightAndCountryID int = NULL output,
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
	INSERT
	INTO [Nop_ShippingByWeightAndCountry]
	(
		ShippingMethodID,
		CountryID,
		[From],
		[To],
		UsePercentage,
		ShippingChargePercentage,
		ShippingChargeAmount
	)
	VALUES
	(
		@ShippingMethodID,
		@CountryID,
		@From,
		@To,
		@UsePercentage,
		@ShippingChargePercentage,
		@ShippingChargeAmount
	)

	set @ShippingByWeightAndCountryID=SCOPE_IDENTITY()
END
