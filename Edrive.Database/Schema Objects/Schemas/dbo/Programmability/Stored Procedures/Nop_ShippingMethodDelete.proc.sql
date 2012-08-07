

CREATE PROCEDURE [dbo].[Nop_ShippingMethodDelete]
(
	@ShippingMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ShippingMethod]
	WHERE
		ShippingMethodID = @ShippingMethodID
END
