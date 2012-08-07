---[dbo].[Nop_SellerCnt] 21212,'',39.356206,40.8038029889983,-82.5916924639456,-76.610989
--EXEC [dbo].[Nop_SellerCnt] 1901,'',42.461246,43.9088429889993,-75.1770877538163,-70.946743
--EXEC [dbo].[Nop_SellerCnt] '21701-0368','',0,0,0,0
CREATE PROCEDURE [dbo].[Nop_SellerCnt] ---1901
(
	@ZipCode				nvarchar(200),
	@Make                   nvarchar(200),
	@Lat1					float,
	@Lat2					float,
	@Long1					float,
	@Long2					float
)
AS
BEGIN

	
	DECLARE @cnt as int
	DECLARE @Flag as bit
	Declare @Paramlist as nvarchar(500)
		
	If Object_Id('tempdb..#TempOptions') Is Not Null 
	Drop Table #TempOptions

	Create Table #TempOptions
	(
		Id Bigint IDENTITY(1,1),
		zip_code varchar(MAX)
	)    
	
	If (@Lat1=0 and @Lat2=0 and @Long1=0 and @Long2=0)
	Begin
		Set @Flag=0
	End
	Else
	Begin
		Insert Into #TempOptions select zip_code from ED_Zipcodes where (latitude between @Lat1 and @Lat2) and (longitude between @Long1 and @Long2)
 
 		select @cnt = COUNT(*)  from #TempOptions
 		
 		if (@Lat1=@Lat2 and @Long1=@Long2)
 			Begin
 				Set @cnt=0
 			End
 			 		
 		Set @Flag=1
	End 
	
     DECLARE @Sql as nvarchar(MAX)
     DECLARE @Sq2 as nvarchar(MAX)
     DECLARE @Sq3 as nvarchar(MAX)
     
	 SET @Sql =	'SELECT 0 as Seller,Count(p.[CustomerID]) as Dealer FROM Nop_Product p 
				 left join Nop_CustomerAttribute as ca
				 on p.CustomerID =ca.CustomerId and ca.[Key] =''ZipPostalCode''
                 WHERE p.Published = 1 AND p.Deleted=0 ' 
				 
				 if(@cnt = 0 and @Flag=1)
				 Begin
			     if(@Zipcode != '0' And @Zipcode != '')
				 Begin
					SET @Sql = @Sql + ' AND ca.Value = ''' + Convert(varchar(max),@ZipCode) + ''' '
				End
			    Else
				Begin
					SET @Sql = @Sql
				End
			End
			Else if(@cnt!=0 and @Flag=1)
			Begin
				SET @Sql = @Sql + ' AND ca.Value in ( select zip_code from #TempOptions) '
			End
			Else if(@Flag=0)
			Begin
				if(@Zipcode != '0' And @Zipcode != '')
				Begin
					SET @Sql = @Sql + ' AND ca.Value = ''' + Convert(varchar(max),@ZipCode) + ''' '
				End
			Else
				Begin
					SET @Sql = @Sql
				End
			End
			Else
			Begin
				SET @Sql = @Sql
			End
			
			 
			if(@Make != '')
			Begin
			SET @Sql = @Sql + 'AND p.Make =''' + @Make + ''' '
			End
			Else
			Begin
	 	    SET @Sql = @Sql
			End
				 
	SET @Sq2 ='  union all		 
				 SELECT Count(p.[CustomerID]) as Seller, 0 as Dealer  FROM Nop_Product p 
				 WHERE p.CustomerID =0 
				 
				'
				 
		 
				if(@cnt = 0 and @Flag=1)
		Begin
			if(@Zipcode != '0' And @Zipcode != '')
				Begin
					SET @Sq2 = @Sq2 + ' AND p.Dealer_Zip  = '''+ Convert(varchar(max),@ZipCode) + ''''
				End
			Else
				Begin
					SET @Sq2 = @Sq2
				End
		End
	Else if(@cnt!=0 and @Flag=1)
		Begin
			SET @Sq2 = @Sq2 + ' AND p.Dealer_Zip in ( select zip_code from #TempOptions) '
		End
	Else if(@Flag=0)
		Begin
			if(@Zipcode != '0' And @Zipcode != '')
				Begin
					SET @Sq2 = @Sq2 + ' AND p.Dealer_Zip  = '''+ Convert(varchar(max),@ZipCode) + ''' '
				End
			Else
				Begin
					SET @Sq2 = @Sq2
				End
		End
	Else
		Begin
			SET @Sq2 = @Sq2
		End
	 
				if(@Make != '')
	 Begin
		SET @Sq2 = @Sq2 + 'AND p.Make =''' + @Make + ''' '
	 End
	 Else
	 Begin
	 	SET @Sq2 = @Sq2
	 End
	 	 

	SET @Sq3 = @Sql + @Sq2
	PRINT(@Sq3)
	
	
	set @Paramlist = '@ZipCode nvarchar(200) ,@Make nvarchar(200),@Lat1 float,@Lat2 float,@Long1 float,@Long2 float'
	exec sp_executesql @Sq3,@Paramlist,@ZipCode,@Make,@Lat1,@Lat2,@Long1,@Long2

END