-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by state
-- =============================================

CREATE PROCEDURE [dbo].[ED_TopFourCity]
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT top(4)city.CityId ,city.Name as cityName,
	sp.Abbreviation as abbrevation
	
	FROM [ED_City] as city
	
	Left join Nop_StateProvince as sp
	on city.StateProvinceID =sp.StateProvinceID
	
	WHERE  deleted=0 and city.ShowOnHomepage=1
	ORDER BY city.DisplayOrder,CityId desc
END