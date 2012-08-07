

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightDelete]
(
	@ShippingByWeightID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ShippingByWeight]
	WHERE
		ShippingByWeightID = @ShippingByWeightID
END
