

CREATE PROCEDURE [dbo].[Nop_ShippingByTotalDelete]
(
	@ShippingByTotalID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ShippingByTotal]
	WHERE
		ShippingByTotalID = @ShippingByTotalID
END
