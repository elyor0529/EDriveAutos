-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <9/3/2011>
-- Description:	Load all products by customer id
-- =============================================


--Modify by henisha rathod 7-7-2011
--exec [dbo].[ED_ProductLoadAllByCustomerID] 37,null,null,null,'Troy'
CREATE PROCEDURE [dbo].[ED_ProductLoadAllByCustomerID] --37,null,null
(
	@CustomerID			int = 0,
	@VIN				nvarchar(MAX),
	@Stock				nvarchar(MAX),
	@Make				nvarchar(MAX),
	@CompanyName				nvarchar(MAX)
	--@Feature            bit
)
AS
BEGIN

	Declare @Sql as nvarchar(MAX)
	Declare @Paramlist as nvarchar(500)
	Declare @finalSql as nvarchar(max)
	Declare @Temp as varchar(5)
	select @Temp = '%'
	
	
	set @Sql = 'SELECT p.[ProductId],p.[Year] + '' '' + p.[Name] as Name,p.[Published],p.[Deleted],p.[CreatedOn],p.[UpdatedOn],p.[VIN],p.[Stock],
		p.[CustomerID],	p.[VehicleName],p.[IsNew],p.[IsFeature],p.[Price_Current],p.[QualifyPrice] FROM Nop_Product p with (NOLOCK) LEFT OUTER JOIN 
		Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID 
		LEFT OUTER JOIN Nop_Customer as c on p.[CustomerID] = c.[CustomerID] 
		LEFT OUTER JOIN Nop_CustomerAttribute as ca on ca.[CustomerID] = c.[CustomerID] AND ca.[Key]=''Company''	
		WHERE p.Published = 1 AND pv.Published = 1 AND p.Deleted = 0  '
				
		
		If(@CustomerID != '-1')
		Begin
			--set @Sql = @Sql + ' And @CustomerID IS NULL OR @CustomerID=0 OR p.CustomerID=@CustomerID '
			If (@CustomerID!=37)
			Begin
				set @Sql = @Sql + ' And p.CustomerID=@CustomerID '
			End 

		end
	Else
		Begin 
			set @Sql = @Sql
		End
		
		If(@VIN != '')
		Begin
			set @Sql = @Sql + ' And p.[VIN] = '''+@VIN+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end
				
		If(@Stock != '')
		Begin
			set @Sql = @Sql + ' And p.[Stock] like ''%'+@Stock+'%'' '
		end
	else
		Begin
			set @Sql = @Sql
		End	
		
		If(@Make != '')
		Begin
			set @Sql = @Sql + ' And p.[Make] = '''+@Make+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end
		
		
		If(@CompanyName != '')
		Begin
			set @Sql = @Sql + ' And ca.[Value] = '''+@CompanyName+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end
	
	--If(@Feature = 'FALSE')
	--	Begin
	--		set @Sql = @Sql + ' And p.[IsFeature] = 0 ' 
	--	End
	--	Else
	--	Begin
	--		set @Sql = @Sql + ' And p.[IsFeature] = 1 '
	--	end
		
	
				
	set @Sql = @Sql + 'ORDER BY	p.[Name] asc'
	
	print(@sql)
	set @Paramlist = '@CustomerID int,@VIN varchar(200),@Stock varchar(200),@Make varchar(200)'
	exec sp_executesql @Sql,@Paramlist,@CustomerID,@VIN,@Stock,@Make

END