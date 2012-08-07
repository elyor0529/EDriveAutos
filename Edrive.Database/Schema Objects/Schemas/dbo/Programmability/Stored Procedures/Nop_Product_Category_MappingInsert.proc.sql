

CREATE PROCEDURE [dbo].[Nop_Product_Category_MappingInsert]
(
	@ProductCategoryID int = NULL output,
	@ProductID int,	
	@CategoryID int,
	@IsFeaturedProduct	bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Product_Category_Mapping]
	(
		ProductID,
		CategoryID,
		IsFeaturedProduct,
		DisplayOrder
	)
	VALUES
	(
		@ProductID,
		@CategoryID,
		@IsFeaturedProduct,
		@DisplayOrder
	)

	set @ProductCategoryID=SCOPE_IDENTITY()
END
