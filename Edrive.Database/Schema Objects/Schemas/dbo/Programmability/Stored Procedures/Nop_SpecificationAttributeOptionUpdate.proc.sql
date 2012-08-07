

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionUpdate]

	@SpecificationAttributeOptionID int,
	@SpecificationAttributeID int,
	@Name nvarchar(500),
	@DisplayOrder int,
	@AttributeOptionFrom int,
	@AttributeOptionTo int

AS
BEGIN

	UPDATE Nop_SpecificationAttributeOption SET
		SpecificationAttributeID = @SpecificationAttributeID,
		Name = @Name,
		DisplayOrder = @DisplayOrder,
		AttributeOptionFrom = @AttributeOptionFrom,
		AttributeOptionTo = @AttributeOptionTo	
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END

