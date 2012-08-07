

CREATE PROCEDURE [dbo].[Nop_ShippingByTotalLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ShippingByTotal]
	ORDER BY ShippingMethodID, [From]
END
