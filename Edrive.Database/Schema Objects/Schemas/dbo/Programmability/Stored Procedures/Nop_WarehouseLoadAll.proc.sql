

CREATE PROCEDURE [dbo].[Nop_WarehouseLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Warehouse]
	WHERE Deleted=0
END
