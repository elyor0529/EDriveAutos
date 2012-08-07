-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <9/3/2011>
-- Description:	Load all products by customer id
-- =============================================


--Modify by henisha rathod 7-7-2011

CREATE PROCEDURE [dbo].[ED_ProductLoadAllByCustomerIDNew] --37,'',''
(
	@CustomerID			int = 0,
	@VIN				nvarchar(MAX),
	@Stock				nvarchar(MAX)
)
AS
BEGIN

	Declare @Sql as nvarchar(500)
	Declare @Paramlist as nvarchar(500)
	Declare @finalSql as nvarchar(max)
	Declare @Temp as varchar(5)
	select @Temp = '%'
	
	
	set @Sql = 'SELECT p.[ProductId],p.[Name],p.[Published],p.[Deleted],p.[CreatedOn],p.[UpdatedOn],p.[VIN],
		p.[CustomerID],	p.[VehicleName],p.[IsNew] FROM Nop_Product p with (NOLOCK) LEFT OUTER JOIN 
		Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID WHERE p.Published = 1 AND pv.Published = 1 AND p.Deleted=0'
				
		
		If(@CustomerID != '-1')
		Begin
			set @Sql = @Sql + ' And p.CustomerID=@CustomerID '
		end
	Else
		Begin 
			set @Sql = @Sql
		End
		
		If(@VIN != '')
		Begin
			set @Sql = @Sql + ' And p.VIN like  @VIN ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end
				
		If(@Stock != '')
		Begin
			set @Sql = @Sql + ' And p.Stock = @Stock '
		end
	else
		Begin
			set @Sql = @Sql
		End	
	
				
	set @Sql = @Sql + 'ORDER BY	p.[Name] asc'
	
	print(@sql)
	set @Paramlist = '@CustomerID int,@VIN varchar(200),@Stock varchar(200)'
	exec sp_executesql @Sql,@Paramlist,@CustomerID,@VIN,@Stock
	
END






