

CREATE PROCEDURE [dbo].[Nop_Product_Category_MappingDelete]
(
	@ProductCategoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Product_Category_Mapping]
	WHERE
		ProductCategoryID = @ProductCategoryID
END
