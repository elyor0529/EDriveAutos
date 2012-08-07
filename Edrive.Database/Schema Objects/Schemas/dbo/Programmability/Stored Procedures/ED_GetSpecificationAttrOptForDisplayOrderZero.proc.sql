-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <25/3/2011>
-- Description:	For getting Specification Attribute Options for Dropdown
-- ================================================

CREATE PROCEDURE [dbo].[ED_GetSpecificationAttrOptForDisplayOrderZero] --1
	@SpecificationAttributeID int

AS
BEGIN
	Select Z.SpecificationAttributeID,
			Z.DisplayOrder,Z.DisplayOrderNew,
			Z.SpecificationAttributeOptionID,Z.[SpecificationAttributeRange],
			CASE Z.Name
				WHEN '' THEN [SpecificationAttributeRange]
				ELSE Z.Name 
			END as [SpecificationAttributeOptionName] 
	From
	(
		SELECT 
			sa.SpecificationAttributeID,
			sa.DisplayOrder,sao.DisplayOrder As DisplayOrderNew,
			sao.SpecificationAttributeOptionID,
			CASE 
				WHEN sao.AttributeOptionTo = 999999999 And sa.SpecificationAttributeID = 5
					THEN '$' + (Convert(varchar(50),(sao.AttributeOptionFrom - 1))+ '+')
				WHEN sao.AttributeOptionTo = 999999999 
					THEN (Convert(varchar(50),(sao.AttributeOptionFrom - 1))+ '+')				
				WHEN sa.SpecificationAttributeID = 5
					THEN '$' + (Convert(varchar(50),sao.AttributeOptionFrom) + ' - ' + '$' + Convert(varchar(50),sao.AttributeOptionTo))
				ELSE (Convert(varchar(50),sao.AttributeOptionFrom) + ' - ' + Convert(varchar(50),sao.AttributeOptionTo))
			End As [SpecificationAttributeRange],sao.Name
		FROM Nop_SpecificationAttributeOption sao
			INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
				sa.SpecificationAttributeID = sao.SpecificationAttributeID	
		WHERE 
			sao.SpecificationAttributeID = @SpecificationAttributeID 
		GROUP BY
			sa.SpecificationAttributeID, 
			sa.DisplayOrder,sao.DisplayOrder,
			sao.SpecificationAttributeOptionID,sao.AttributeOptionFrom,
			sao.AttributeOptionTo,
			sao.Name		
	) As Z
	UNION 
	Select 0,1,0,0,'All','All' FROM Nop_SpecificationAttributeOption
	ORDER BY  DisplayOrder, SpecificationAttributeID,SpecificationAttributeOptionName,
			DisplayOrderNew
END