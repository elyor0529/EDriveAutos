

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]

	@SpecificationAttributeOptionID int,
	@LanguageID int

AS
BEGIN

	SELECT
		sao.SpecificationAttributeOptionID, 
		sao.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(saol.Name,sao.Name) as [Name],
		sao.DisplayOrder,sao.AttributeOptionFrom,
		sao.AttributeOptionTo
	FROM Nop_SpecificationAttributeOption sao
		LEFT OUTER JOIN [Nop_SpecificationAttributeOptionLocalized] saol with (NOLOCK) ON 
			sao.SpecificationAttributeOptionID = saol.SpecificationAttributeOptionID AND saol.LanguageID = @LanguageID	
	WHERE 
		sao.SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END

