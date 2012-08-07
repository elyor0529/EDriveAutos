

CREATE PROCEDURE [dbo].[Nop_ProductTag_Product_MappingDelete]
(
	@ProductTagID int,
	@ProductID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductTag_Product_Mapping]
	WHERE
		[ProductTagID] = @ProductTagID and [ProductID]=@ProductID
	
	exec [Nop_ProductTagUpdateCounts] @ProductTagID
END
