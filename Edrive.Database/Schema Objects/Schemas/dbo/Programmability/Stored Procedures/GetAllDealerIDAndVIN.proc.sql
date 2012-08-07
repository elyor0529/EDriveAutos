-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Insert City
-- =============================================

CREATE PROCEDURE [dbo].[GetAllDealerIDAndVIN]

AS
BEGIN
	
	Declare @iDays as int
Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'

select p.VIN  as VIN , c.CustomerID as DealerID from Nop_Customer as c
left join
Nop_Product as p 
on c.CustomerID = p.CustomerId  
where p.Deleted = 0 and p.Published =1 and c.Deleted=0 and c.Active=1 
and DATEDIFF(s,getdate(),DATEADD(d, @iDays ,UpdatedOn)) > 0

END