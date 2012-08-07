-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by state
-- =============================================
---exec [dbo].[ED_CityLoadByName] 'New York'
Create PROCEDURE [dbo].[ED_CityCheckLoadByName]
	@Name varchar(Max)
AS
BEGIN
	SET NOCOUNT ON
	
	select * from ED_City as city
	where city.Name=@Name
	and city.Deleted=0 
END