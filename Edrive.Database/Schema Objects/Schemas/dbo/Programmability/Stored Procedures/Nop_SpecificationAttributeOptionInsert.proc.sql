

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionInsert]

	@SpecificationAttributeOptionID int = NULL OUTPUT,
	@SpecificationAttributeID int,
	@Name nvarchar(500),
	@DisplayOrder int,
	@AttributeOptionFrom int,
	@AttributeOptionTo int

AS
BEGIN

	INSERT INTO [Nop_SpecificationAttributeOption] 
	(
		SpecificationAttributeID,
		Name,
		DisplayOrder,
		AttributeOptionFrom,
		AttributeOptionTo
	)
	VALUES 
	(
		@SpecificationAttributeID,
		@Name,
		@DisplayOrder,
		@AttributeOptionFrom,
		@AttributeOptionTo
	)
	
	SET @SpecificationAttributeOptionID = SCOPE_IDENTITY()

END


