

CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingDelete]
(
	@ProductSpecificationAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Product_SpecificationAttribute_Mapping]
	WHERE
		ProductSpecificationAttributeID = @ProductSpecificationAttributeID
END
