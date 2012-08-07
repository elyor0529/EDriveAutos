

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeInsert]
(
	@SpecificationAttributeID int = NULL output,
	@Name nvarchar(400),
	@DisplayOrder int
)
AS
BEGIN
	
	INSERT
	INTO [Nop_SpecificationAttribute]
	(
		[Name],
		DisplayOrder
	)
	VALUES
	(
		@Name,
		@DisplayOrder
	)

	SET @SpecificationAttributeID = SCOPE_IDENTITY()
	
END
