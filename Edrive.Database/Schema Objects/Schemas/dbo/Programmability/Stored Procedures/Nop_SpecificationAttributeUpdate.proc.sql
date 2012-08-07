

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeUpdate]
(
	@SpecificationAttributeID int,
	@Name nvarchar(400),
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_SpecificationAttribute] SET
		[Name] = @Name,
		DisplayOrder = @DisplayOrder
	WHERE
		SpecificationAttributeID = @SpecificationAttributeID
END
