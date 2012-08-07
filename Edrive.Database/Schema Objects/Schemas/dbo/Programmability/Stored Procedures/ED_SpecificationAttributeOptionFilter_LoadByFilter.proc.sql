-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <14/3/2011>
-- Description:	For getting Specification Attribute And Attribute Options
-- ================================================

CREATE PROCEDURE [dbo].[ED_SpecificationAttributeOptionFilter_LoadByFilter] 

AS
BEGIN
	Select Z.SpecificationAttributeID,
			Z.[SpecificationAttributeName],
			Z.DisplayOrder,Z.DisplayOrderNew,
			Z.SpecificationAttributeOptionID,Z.AttributeOptionFrom,
			Z.AttributeOptionTo,Z.[SpecificationAttributeRange],
			CASE Z.Name
				WHEN '' THEN [SpecificationAttributeRange]
				ELSE Z.Name 
			END as [SpecificationAttributeOptionName] 
	From
	(
		SELECT 
			sa.SpecificationAttributeID,
			sa.Name As [SpecificationAttributeName],
			sa.DisplayOrder,sao.DisplayOrder As DisplayOrderNew,
			sao.SpecificationAttributeOptionID,sao.AttributeOptionFrom,
			sao.AttributeOptionTo,
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
				sa.SpecificationAttributeID = sao.SpecificationAttributeID	and sao.DisplayOrder =1
	--	WHERE 
	--		sao.AllowFiltering = 1
--Added By Jinal To remove ZipCode from Search.aspx
		WHERE
			sa.SpecificationAttributeID != 6
		GROUP BY
			sa.SpecificationAttributeID, 
			sa.Name,
			sa.DisplayOrder,sao.DisplayOrder,
			sao.SpecificationAttributeOptionID,sao.AttributeOptionFrom,
			sao.AttributeOptionTo,
			sao.Name		
	) As Z
--	UNION 
--	Select 0,'',1,0,0,0,'','Any' FROM Nop_SpecificationAttributeOption
	ORDER BY  Z.DisplayOrder, Z.SpecificationAttributeID,
			Z.[SpecificationAttributeName],Z.DisplayOrderNew, Z.[Name]
END