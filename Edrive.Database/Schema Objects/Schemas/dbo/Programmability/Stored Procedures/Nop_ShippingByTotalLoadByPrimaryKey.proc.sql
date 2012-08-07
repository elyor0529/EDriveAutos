

CREATE PROCEDURE [dbo].[Nop_ShippingByTotalLoadByPrimaryKey]
(
	@ShippingByTotalID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingByTotal]
	WHERE
		ShippingByTotalID = @ShippingByTotalID
END
