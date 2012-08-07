

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeDelete]
(
	@SpecificationAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_SpecificationAttribute]
	WHERE
		SpecificationAttributeID = @SpecificationAttributeID
END
