

CREATE PROCEDURE [dbo].[Nop_WarehouseLoadByPrimaryKey]
(
	@WarehouseID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Warehouse]
	WHERE
		(WarehouseID = @WarehouseID)
END
