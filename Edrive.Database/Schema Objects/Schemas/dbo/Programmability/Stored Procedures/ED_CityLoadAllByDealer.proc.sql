-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by state
-- =============================================

CREATE PROCEDURE [dbo].[ED_CityLoadAllByDealer]
(
@CityId int
)	
AS
BEGIN
	SET NOCOUNT ON
	select ca.Value as DealerName,city.Name,c.CustomerID from Nop_Customer as c 
	inner join 
	ED_City as city
	on c.DealerCityId = city.CityId
	
	left outer join Nop_CustomerAttribute ca
	on ca.CustomerId = c.CustomerID and ca.[key]='Company'
	
	where c.Active=1 and city.Deleted =0 and city.CityId=@CityId
	
	;
	
	
	
END