

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionDelete]

	@SpecificationAttributeOptionID int

AS
BEGIN

	DELETE 
	FROM Nop_SpecificationAttributeOption
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END
