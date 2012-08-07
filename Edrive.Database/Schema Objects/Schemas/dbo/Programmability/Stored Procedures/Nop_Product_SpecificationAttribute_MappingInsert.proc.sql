

CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingInsert]
(
	@ProductSpecificationAttributeID int = NULL output,
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

	INSERT	
	INTO [Nop_Product_SpecificationAttribute_Mapping]
	(
		ProductID,
		SpecificationAttributeOptionID,
		AllowFiltering,
		ShowOnProductPage,
		DisplayOrder,
		SpecificationAttributeID,
		IsActive
	)
	VALUES
	(
		@ProductID,
		@SpecificationAttributeOptionID,
		@AllowFiltering,
		@ShowOnProductPage,
		@DisplayOrder,
		@SpecificationAttributeID,
		@IsActive
	)

	SET @ProductSpecificationAttributeID = SCOPE_IDENTITY()
	
END

