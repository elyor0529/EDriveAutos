

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightUpdate]
(
	@ShippingByWeightID int,
	@ShippingMethodID int,
	@From decimal(18, 2),
	@To decimal(18, 2),
	@UsePercentage bit,
	@ShippingChargePercentage decimal(18, 2),
	@ShippingChargeAmount decimal(18, 2)
)
AS
BEGIN
	UPDATE [Nop_ShippingByWeight]
	SET
		ShippingMethodID=@ShippingMethodID,
		[From]=@From,
		[To]=@To,
		UsePercentage=@UsePercentage,
		ShippingChargePercentage=@ShippingChargePercentage,
		ShippingChargeAmount=@ShippingChargeAmount
	WHERE
		ShippingByWeightID = @ShippingByWeightID
END
