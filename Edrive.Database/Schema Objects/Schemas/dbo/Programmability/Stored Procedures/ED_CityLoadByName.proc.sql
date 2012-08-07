-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by state
-- =============================================
---exec [dbo].[ED_CityLoadByName] 'Dallas'
CREATE PROCEDURE [dbo].[ED_CityLoadByName]
	@Name varchar(Max)
AS
BEGIN
	SET NOCOUNT ON
	
	select c.CustomerID,ca.Value as company,ca1.Value as address1 ,
	ca2.Value as city ,sp.Abbreviation as satate,ca4.Value as zip,ca5.Value as phone,
	ec.CityImageId,ec.CountryID,ec.StateProvinceID,ec.Name
	from nop_customer as c
	right join 
	ED_City as ec
	on c.DealerCityId =ec.CityId

	right join Nop_CustomerAttribute as ca
	on ca.CustomerId =c.CustomerID and ca.[Key] = 'Company'

	right join Nop_CustomerAttribute as ca1
	on ca1.CustomerId =c.CustomerID and ca1.[Key] = 'StreetAddress'

	right join Nop_CustomerAttribute as ca2
	on ca2.CustomerId =c.CustomerID and ca2.[Key] = 'City'

	right join Nop_CustomerAttribute as ca3
	on ca3.CustomerId =c.CustomerID and ca3.[Key] = 'StateProvinceID'

	right join Nop_StateProvince as sp
	on sp.StateProvinceID = ca3.Value

	right join Nop_CustomerAttribute as ca4
	on ca4.CustomerId =c.CustomerID and ca4.[Key] = 'ZipPostalCode'

	right join Nop_CustomerAttribute as ca5
	on ca5.CustomerId =c.CustomerID and ca5.[Key] = 'PhoneNumber'

	where ec.Name=@Name and c.Deleted=0 and c.Active=1


END