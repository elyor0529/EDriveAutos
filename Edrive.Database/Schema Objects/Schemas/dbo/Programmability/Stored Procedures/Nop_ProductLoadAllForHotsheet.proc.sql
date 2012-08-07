--[dbo].[Nop_ProductLoadAllForHotsheet] '21212','',39.356206,40.8038029889983,-82.5916924639456,76.610989,0,100,0
--[dbo].[Nop_ProductLoadAllForHotsheet] "21212",'',39.356206,40.8038029889983,-82.5916924639456,76.610989,0,10,0
--[dbo].[Nop_ProductLoadAllForHotsheet] '21212','',0,0,0,0,0,15
--EXEC [dbo].[Nop_ProductLoadAllForHotsheet] 1901,'',42.461246,43.9088429889993,-75.1770877538163,-70.946743
CREATE PROCEDURE [dbo].[Nop_ProductLoadAllForHotsheet]
(
	@ZipCode				nvarchar(200),
	@Make                   nvarchar(200),
	@Lat1					float,
	@Lat2					float,
	@Long1					float,
	@Long2					float,
	@PageIndex				int = 0, 
	@PageSize				int = 2147483644,
	@TotalRecords			int = null OUTPUT
)

AS
BEGIN

	DECLARE @cnt as int
	DECLARE @Flag as bit
	
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
     
	 SET @Sql =	'SELECT p.[ProductId],p.[Name] FROM Nop_Product p 
                 left join Nop_CustomerAttribute as ca
				 on p.CustomerID =ca.CustomerId and ca.[Key] =''ZipPostalCode''
				 WHERE p.Published = 1 AND p.Deleted=0 ' 
				 
				 if(@cnt = 0 and @Flag=1)
				 Begin
					
					if(@Zipcode!= '')
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
					if(@Zipcode != '')
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
	
				 
	SET @Sq2 ='  union 				 
                 SELECT p.[ProductId],p.[Name] FROM Nop_Product p 
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
	
	
	CREATE TABLE #DisplayOrderTmp 
	(
		[ID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL,
		[Name] nvarchar(400) not null
	)

	INSERT INTO #DisplayOrderTmp
	EXEC(@Sq3)
	--paging
		
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	DECLARE @TotalThreads int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		[IndexID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL
	)
	
	

	INSERT INTO #PageIndex ([ProductID])
	SELECT ProductID
	FROM #DisplayOrderTmp with (NOLOCK)
	GROUP BY ProductID
	ORDER BY min([ID])
	
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	
	
	SELECT  p.[ProductId],p.[Name],p.[ShortDescription],
			p.[FullDescription],p.[AdminComment],p.[ProductTypeID], 
			p.[TemplateID],p.[ShowOnHomePage],p.[MetaKeywords],
			p.[MetaDescription],p.[MetaTitle],p.[SEName],
			p.[AllowCustomerReviews],p.[AllowCustomerRatings], 
			p.[RatingSum],p.[TotalRatingVotes],p.[Published], p.[Deleted], 
			p.[CreatedOn],p.[UpdatedOn],p.[VIN],p.[CustomerID],p.[VehicleName],
			p.[Type],p.[TypeAttribute],p.[Stock],p.[Year],p.[YearAttribute],p.[Make],
			p.[MakeAttribute],p.[Model],p.[Trim],p.[Free_Text],p.[Body],
			p.[BodyAttribute],p.[Mileage],p.[MileageAttribute],p.[Price_Current],
			p.[Reserved],p.[Price_Wholesale],p.[Price_Cost],p.[PriceAttribute],
			p.[Title],p.[Condition],p.[Exterior_Color],p.[Interior_Color],
			p.[Doors],p.[Engine],p.[Transmission],p.[Fuel_Type],p.[Drive_Type],
			p.[Options],p.[Warranty],p.[WarrantyAttribute],p.[Description],p.[Pics],p.[Dealer_Name],p.[Dealer_Zip],p.[Date_in_Stock],
			p.[FileName],p.[IsNew],p.[IsFeature],p.[SavingAmount],p.[City],p.[StateID] ,p.[QualifyPrice] ,p.[OwnerDetail],p.[Show_on_Dealer],p.[Offer],p.[City_Fuel],p.[Highway_Fuel],p.[AverageRetailPrice],p.[AverageTradeinPrice],
			p.[SellerName] ,p.[SellerEmail] ,p.[SellerPhone] ,p.[SellerNotes] 
			
		
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound 
		
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	
	DROP TABLE #DisplayOrderTmp
	DROP TABLE #PageIndex
	Drop Table #TempOptions
	
END