

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightLoadByShippingMethodID]
(
	@ShippingMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingByWeight]
	WHERE
		ShippingMethodID=@ShippingMethodID
	ORDER BY [From]
END
