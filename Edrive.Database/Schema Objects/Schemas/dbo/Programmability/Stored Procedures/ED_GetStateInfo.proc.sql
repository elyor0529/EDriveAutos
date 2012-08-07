-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <3/4/2011>
-- Description:	For getting Popular Cars
-- ================================================

CREATE PROCEDURE [dbo].[ED_GetStateInfo] 
	
AS
BEGIN

	
	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'

	
	Select Distinct(sao.Name) as Name,sao.StateProvinceID as StateProvinceID,Count(p.ProductId) As List
	From Nop_StateProvince sao
	left join Nop_CustomerAttribute ca on
	sao.StateProvinceID = CONVERT(int,ca.[Value]) and (ca.[Key]='StateProvinceID')
	left join Nop_Customer c on
	c.CustomerID = ca.CustomerId
	left join Nop_Product p
	ON c.CustomerId = p.CustomerID and (DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0) and (p.QualifyPrice is not null) and (p.Deleted=0) 
	where sao.CountryID=1 
	Group By sao.Name,sao.StateProvinceID
	Order By Name asc 
	
END



--select count(ProductId) from nop_product where make='CHEVROLET'