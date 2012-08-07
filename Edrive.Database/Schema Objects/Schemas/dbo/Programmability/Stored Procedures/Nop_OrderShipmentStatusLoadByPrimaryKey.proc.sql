

CREATE PROCEDURE [dbo].[Nop_OrderShipmentStatusLoadByPrimaryKey]
(
	@OrderShipmentStatusID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderShipmentStatus]
	WHERE
		OrderShipmentStatusID = @OrderShipmentStatusID
END
