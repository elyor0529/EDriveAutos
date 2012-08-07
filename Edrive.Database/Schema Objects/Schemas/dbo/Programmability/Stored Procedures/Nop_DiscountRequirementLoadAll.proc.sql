

CREATE PROCEDURE [dbo].[Nop_DiscountRequirementLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_DiscountRequirement]
	ORDER BY DiscountRequirementID
END
