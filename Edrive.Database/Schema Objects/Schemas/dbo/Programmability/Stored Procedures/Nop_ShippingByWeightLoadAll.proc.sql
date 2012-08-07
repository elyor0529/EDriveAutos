

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ShippingByWeight]
	ORDER BY ShippingMethodID, [From]
END
