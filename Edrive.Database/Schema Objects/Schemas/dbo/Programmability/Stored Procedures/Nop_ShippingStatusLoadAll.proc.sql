

CREATE PROCEDURE [dbo].[Nop_ShippingStatusLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingStatus]
	ORDER BY ShippingStatusID
END
