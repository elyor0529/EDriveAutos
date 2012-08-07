

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightLoadByPrimaryKey]
(
	@ShippingByWeightID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingByWeight]
	WHERE
		ShippingByWeightID = @ShippingByWeightID
END
