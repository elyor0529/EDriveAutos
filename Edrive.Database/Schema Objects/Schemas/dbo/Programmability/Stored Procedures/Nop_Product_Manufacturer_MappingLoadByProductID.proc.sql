

CREATE PROCEDURE [dbo].[Nop_Product_Manufacturer_MappingLoadByProductID]
(
	@ProductID int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pmm.*
	FROM Nop_Product_Manufacturer_Mapping pmm
	INNER JOIN Nop_Manufacturer m ON pmm.ManufacturerID=m.ManufacturerID	
	WHERE pmm.ProductID=@ProductID	
		AND (m.Published = 1 or @ShowHidden = 1) and m.Deleted=0
	ORDER BY pmm.DisplayOrder
END
