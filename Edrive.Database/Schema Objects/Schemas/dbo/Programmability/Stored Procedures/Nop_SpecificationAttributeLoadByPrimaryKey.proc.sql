

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLoadByPrimaryKey]
(
	@SpecificationAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		sa.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(sal.Name,sa.Name) as [Name],
		sa.DisplayOrder
	FROM [Nop_SpecificationAttribute] sa
		LEFT OUTER JOIN [Nop_SpecificationAttributeLocalized] sal
		ON sa.SpecificationAttributeID = sal.SpecificationAttributeID AND sal.LanguageID = @LanguageID	
	WHERE
		sa.SpecificationAttributeID = @SpecificationAttributeID
END
