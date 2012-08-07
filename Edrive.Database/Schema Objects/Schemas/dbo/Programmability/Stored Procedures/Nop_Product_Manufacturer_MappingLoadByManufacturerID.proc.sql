

CREATE PROCEDURE [dbo].[Nop_Product_Manufacturer_MappingLoadByManufacturerID]
(
	@ManufacturerID int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pmm.*
	FROM Nop_Product_Manufacturer_Mapping pmm
	INNER JOIN Nop_Product p ON pmm.ProductID=p.ProductID
	WHERE pmm.ManufacturerID=@ManufacturerID
		AND (p.Published = 1 or @ShowHidden = 1) and p.Deleted=0
	ORDER BY pmm.DisplayOrder
END
