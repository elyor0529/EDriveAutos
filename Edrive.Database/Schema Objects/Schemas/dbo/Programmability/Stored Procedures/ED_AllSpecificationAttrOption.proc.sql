-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <25/3/2011>
-- Description:	For getting Specification Attribute Options for Dropdown
-- ================================================

CREATE PROCEDURE [dbo].[ED_AllSpecificationAttrOption] 
	

AS
BEGIN
		SELECT 
		
			sao.SpecificationAttributeOptionID,sao.Name,'Make' as SpecificationAttribute
			
		FROM Nop_SpecificationAttributeOption sao
			INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
				sa.SpecificationAttributeID = sao.SpecificationAttributeID and sao.DisplayOrder =1
		WHERE 
			sao.SpecificationAttributeID = 1 and sao.DisplayOrder = 1
			
			
			union
			
				SELECT 
		
			sao.SpecificationAttributeOptionID,sao.Name,'Body' as SpecificationAttribute
			
		FROM Nop_SpecificationAttributeOption sao
			INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
				sa.SpecificationAttributeID = sao.SpecificationAttributeID and sao.DisplayOrder =1
		WHERE 
			sao.SpecificationAttributeID = 2 and sao.DisplayOrder = 1
			
		--	union
			
		--	SELECT 
		
		--	sao.SpecificationAttributeOptionID,sao.Name,'Year' as SpecificationAttribute
			
		--FROM Nop_SpecificationAttributeOption sao
		--	INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
		--		sa.SpecificationAttributeID = sao.SpecificationAttributeID and sao.DisplayOrder =1
		--WHERE 
		--	sao.SpecificationAttributeID = 3 and sao.DisplayOrder = 1
			
				
		--	union
			
		--	SELECT 
		
		--	sao.SpecificationAttributeOptionID,CASE 
		--		WHEN sao.AttributeOptionTo = 999999999 And sa.SpecificationAttributeID = 5
		--			THEN '$' + (Convert(varchar(50),(sao.AttributeOptionFrom - 1))+ '+')
		--		WHEN sao.AttributeOptionTo = 999999999 
		--			THEN (Convert(varchar(50),(sao.AttributeOptionFrom - 1))+ '+')				
		--		WHEN sa.SpecificationAttributeID = 5
		--			THEN '$' + (Convert(varchar(50),sao.AttributeOptionFrom) + ' - ' + '$' + Convert(varchar(50),sao.AttributeOptionTo))
		--		ELSE (Convert(varchar(50),sao.AttributeOptionFrom) + ' - ' + Convert(varchar(50),sao.AttributeOptionTo))
		--	End As [SpecificationAttribute],'Price' 
			
		--FROM Nop_SpecificationAttributeOption sao
		--	INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
		--		sa.SpecificationAttributeID = sao.SpecificationAttributeID and sao.DisplayOrder =1
		--WHERE 
		--	sao.SpecificationAttributeID = 5
		
		
			union
			
			SELECT 
		
			sao.SpecificationAttributeOptionID,CASE 
				WHEN sao.AttributeOptionTo = 999999999 And sa.SpecificationAttributeID = 5
					THEN (Convert(varchar(50),(sao.AttributeOptionFrom - 1))+ '+')
				WHEN sao.AttributeOptionTo = 999999999 
					THEN (Convert(varchar(50),(sao.AttributeOptionFrom - 1))+ '+')				
				WHEN sa.SpecificationAttributeID = 4
					THEN (Convert(varchar(50),sao.AttributeOptionFrom) + ' - ' + Convert(varchar(50),sao.AttributeOptionTo))
				ELSE (Convert(varchar(50),sao.AttributeOptionFrom) + ' - ' + Convert(varchar(50),sao.AttributeOptionTo))
			End As [SpecificationAttribute],'Mileage' 
			
		FROM Nop_SpecificationAttributeOption sao
			INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
				sa.SpecificationAttributeID = sao.SpecificationAttributeID and sao.DisplayOrder =1
		WHERE 
			sao.SpecificationAttributeID = 4
		
		
		
	
END