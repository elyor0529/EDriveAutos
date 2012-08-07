

CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingLoadByProductID]
(
	@ProductID int,
	@AllowFiltering bit,
	@ShowOnProductPage bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		psam.*
	FROM Nop_Product_SpecificationAttribute_Mapping psam
	WHERE psam.ProductID = @ProductID
		AND (@AllowFiltering IS NULL OR psam.AllowFiltering=@AllowFiltering)
		AND (@ShowOnProductPage IS NULL OR psam.ShowOnProductPage=@ShowOnProductPage)
	ORDER BY psam.DisplayOrder
END
