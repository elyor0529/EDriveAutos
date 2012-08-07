

CREATE PROCEDURE [dbo].[Nop_Product_Category_MappingUpdate]
(
	@ProductCategoryID int,
	@ProductID int,	
	@CategoryID int,
	@IsFeaturedProduct	bit,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_Product_Category_Mapping]
	SET
		ProductID=@ProductID,
		CategoryID=@CategoryID,
		IsFeaturedProduct=@IsFeaturedProduct,
		DisplayOrder=@DisplayOrder
	WHERE
		ProductCategoryID = @ProductCategoryID

END
