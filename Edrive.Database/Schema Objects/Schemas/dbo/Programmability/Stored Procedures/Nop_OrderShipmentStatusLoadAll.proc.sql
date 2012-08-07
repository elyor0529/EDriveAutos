

CREATE PROCEDURE [dbo].[Nop_OrderShipmentStatusLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderShipmentStatus]
	ORDER BY OrderShipmentStatusID
END
