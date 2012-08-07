-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <3/4/2011>
-- Description:	For getting Popular Cars
-- ================================================

CREATE PROCEDURE [dbo].[ED_GetPopularCars] 
	
AS
BEGIN
--	Select Distinct(Make),MakeAttribute,Count(ProductId) As List
--	From Nop_Product
--	Group By Make,MakeAttribute
--	Order By List DESC

Declare @iDays as int
Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'
print @iDays

Select Distinct(sao.Name) as Make,sao.SpecificationAttributeOptionId as MakeAttribute,Count(p.ProductId) As List
	From Nop_SpecificationAttributeOption sao
	LEFT JOIN Nop_Product p
	ON sao.SpecificationAttributeOptionId = p.MakeAttribute and (p.Deleted = 0) and (DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 ) and ( p.QualifyPrice is not null )
	
	right join Nop_Customer c on p.CustomerID = c.CustomerID 
	
	right join Nop_CustomerAttribute ca on ca.CustomerId = c.CustomerID and ca.[Key] = 'StateProvinceID'
	--			right join Nop_CustomerAttribute caa on caa.CustomerId = c.CustomerID and caa.[Key] = 'ZipPostalCode'
	--			right join Nop_CustomerAttribute cab on cab.CustomerId = c.CustomerID and cab.[Key] = 'City'
	
	WHERE sao.SpecificationAttributeID = 1  and c.Deleted=0
	Group By sao.Name,sao.SpecificationAttributeOptionId
	Order By List DESC
END



--select count(ProductId) from nop_product where make='CHEVROLET'