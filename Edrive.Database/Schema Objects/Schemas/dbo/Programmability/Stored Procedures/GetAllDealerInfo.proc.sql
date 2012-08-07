-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Insert City
-- =============================================

CREATE PROCEDURE [dbo].[GetAllDealerInfo]

AS
BEGIN

Declare @iDays as int
Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'

	
select distinct c.CustomerID as DealerID ,ca.[Value] as DealerName ,
ca2.[Value] as Address1 ,ca3.[Value] as Address2 ,ca4.[Value] as City , sp.Name as [State] , ca6.[Value] as ZipCode ,
ca7.[Value] as PhoneNumber
from Nop_Customer as c

left join
Nop_CustomerAttribute as ca
on c.CustomerID =ca.CustomerId and ca.[Key] = 'Company'

left join 
Nop_CustomerAttribute as ca2
on c.CustomerID =ca2.CustomerId and ca2.[Key] = 'StreetAddress'

left join 
Nop_CustomerAttribute as ca3
on c.CustomerID =ca3.CustomerId and ca3.[Key] = 'StreetAddress2'

left join 
Nop_CustomerAttribute as ca4
on c.CustomerID =ca4.CustomerId and ca4.[Key] = 'City'


left join 
Nop_CustomerAttribute as ca5
on c.CustomerID =ca5.CustomerId and ca5.[Key] = 'StateProvinceID'

right join 
Nop_StateProvince as sp 
on ca5.[Value] = sp.StateProvinceID

left join 
Nop_CustomerAttribute as ca6
on c.CustomerID =ca6.CustomerId and ca6.[Key] = 'ZipPostalCode'

left join 
Nop_CustomerAttribute as ca7
on c.CustomerID =ca7.CustomerId and ca7.[Key] = 'PhoneNumber'

left join
Nop_Product as p
on p.CustomerID = c.CustomerID 

where c.CustomerType =1 and c.Active =1 and c.Deleted=0 and p.Deleted = 0 and p.Published = 1 and DATEDIFF(s,getdate(),DATEADD(d, @iDays ,UpdatedOn)) > 0

END