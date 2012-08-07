

CREATE PROCEDURE [dbo].[Nop_Product_Category_MappingLoadByPrimaryKey]
(
	@ProductCategoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Product_Category_Mapping]
	WHERE
		ProductCategoryID = @ProductCategoryID
END
