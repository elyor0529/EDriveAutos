

CREATE PROCEDURE [dbo].[Nop_ShippingByTotalUpdate]
(
	@ShippingByTotalID int,
	@ShippingMethodID int,
	@From decimal(18, 2),
	@To decimal(18, 2),
	@UsePercentage bit,
	@ShippingChargePercentage decimal(18, 2),
	@ShippingChargeAmount decimal(18, 2)
)
AS
BEGIN
	UPDATE [Nop_ShippingByTotal]
	SET
		ShippingMethodID=@ShippingMethodID,
		[From]=@From,
		[To]=@To,
		UsePercentage=@UsePercentage,
		ShippingChargePercentage=@ShippingChargePercentage,
		ShippingChargeAmount=@ShippingChargeAmount
	WHERE
		ShippingByTotalID = @ShippingByTotalID
END
