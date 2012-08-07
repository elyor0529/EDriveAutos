

CREATE PROCEDURE [dbo].[Nop_ShippingByTotalInsert]
(
	@ShippingByTotalID int = NULL output,
	@ShippingMethodID int,
	@From decimal(18, 2),
	@To decimal(18, 2),
	@UsePercentage bit,
	@ShippingChargePercentage decimal(18, 2),
	@ShippingChargeAmount decimal(18, 2)
)
AS
BEGIN
	INSERT
	INTO [Nop_ShippingByTotal]
	(
		ShippingMethodID,
		[From],
		[To],
		UsePercentage,
		ShippingChargePercentage,
		ShippingChargeAmount
	)
	VALUES
	(
		@ShippingMethodID,
		@From,
		@To,
		@UsePercentage,
		@ShippingChargePercentage,
		@ShippingChargeAmount
	)

	set @ShippingByTotalID=SCOPE_IDENTITY()
END
