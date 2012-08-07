/*********************************
* Updated By : Manali 
* Updated On : 10/3/2011 
* Description : For adding two new fields for filtering purpose.
**********************************/

CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingUpdate]
(
	@ProductSpecificationAttributeID int,
	@ProductID int,	
	@SpecificationAttributeOptionID int,
	@AllowFiltering bit,
	@ShowOnProductPage bit,
	@DisplayOrder int,
	@SpecificationAttributeID int,
	@IsActive bit
)
AS
BEGIN

	UPDATE [Nop_Product_SpecificationAttribute_Mapping]
	SET
		ProductID = @ProductID,
		SpecificationAttributeOptionID = @SpecificationAttributeOptionID,
		AllowFiltering = @AllowFiltering,
		ShowOnProductPage = @ShowOnProductPage,
		DisplayOrder=@DisplayOrder,
		SpecificationAttributeID=@SpecificationAttributeID,
		IsActive=@IsActive
	WHERE
		ProductSpecificationAttributeID = @ProductSpecificationAttributeID

END

