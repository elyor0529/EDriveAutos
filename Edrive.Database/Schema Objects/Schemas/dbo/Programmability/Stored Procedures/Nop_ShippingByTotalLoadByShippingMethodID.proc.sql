

CREATE PROCEDURE [dbo].[Nop_ShippingByTotalLoadByShippingMethodID]
(
	@ShippingMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingByTotal]
	WHERE
		ShippingMethodID=@ShippingMethodID
	ORDER BY [From]
END
