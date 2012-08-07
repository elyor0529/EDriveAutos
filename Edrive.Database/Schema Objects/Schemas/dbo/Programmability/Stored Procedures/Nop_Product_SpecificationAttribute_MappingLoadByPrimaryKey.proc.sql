

CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingLoadByPrimaryKey]
(
	@ProductSpecificationAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Product_SpecificationAttribute_Mapping]
	WHERE
		ProductSpecificationAttributeID = @ProductSpecificationAttributeID
END
