-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by state
-- =============================================

CREATE PROCEDURE [dbo].[ED_CityLoadAllByState]
	@StateProvinceID int
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT *
	FROM [ED_City]
	WHERE StateProvinceID=@StateProvinceID and deleted=0
	ORDER BY DisplayOrder
END