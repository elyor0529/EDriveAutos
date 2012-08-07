CREATE PROCEDURE [dbo].[ED_ProductLoadAllByFeature] --37,null,null

AS
BEGIN

	
	SELECT p.[ProductId],p.[Name],p.[Published],p.[Deleted],p.[CreatedOn],
	p.[UpdatedOn],p.[VIN],p.[Stock],
		p.[CustomerID],	p.[VehicleName],p.[IsNew],p.[Price_Current],p.[IsFeature]  FROM Nop_Product p with (NOLOCK) LEFT OUTER JOIN 
		Nop_ProductVariant 
		pv with (NOLOCK) ON p.ProductID = pv.ProductID WHERE
		 p.Published = 1 AND pv.Published = 1 AND p.Deleted = '0'
			And p.[IsFeature] = 1 ORDER BY	p.[Name] asc
	
END