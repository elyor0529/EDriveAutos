

CREATE PROCEDURE [dbo].[Nop_ProductTag_Product_MappingInsert]
(
	@ProductTagID int,
	@ProductID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_ProductTag_Product_Mapping] WHERE [ProductTagID] = @ProductTagID and [ProductID]=@ProductID)
	INSERT
		INTO [Nop_ProductTag_Product_Mapping]
		(
			[ProductTagID],
			[ProductID]
		)
		VALUES
		(
			@ProductTagID,
			@ProductID
		)

	exec [Nop_ProductTagUpdateCounts] @ProductTagID
END
