-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get City by state
-- =============================================

CREATE PROCEDURE [dbo].[ED_GetTotalProduct]
AS
BEGIN
	SET NOCOUNT ON
	
	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'

	SELECT Count(*) as TotalCar
	FROM Nop_Product p
	
	right join Nop_Customer c on p.CustomerID = c.CustomerID 
	right join Nop_CustomerAttribute ca on ca.CustomerId = c.CustomerID and ca.[Key] = 'StateProvinceID'
				right join Nop_CustomerAttribute caa on caa.CustomerId = c.CustomerID and caa.[Key] = 'ZipPostalCode'
				right join Nop_CustomerAttribute cab on cab.CustomerId = c.CustomerID and cab.[Key] = 'City'
	WHERE 
		p.Published = 1 AND p.Deleted=0 and (DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0) 
		And p.QualifyPrice is not null

END