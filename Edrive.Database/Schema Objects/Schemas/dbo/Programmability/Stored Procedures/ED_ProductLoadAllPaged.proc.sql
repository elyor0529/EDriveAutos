-- =======================================================================
-- Author:		<Manali Panchal>
-- Create date: <15/3/2011>
-- Description:	For getting all products search by specification attribute
-- Update date: <30/3/2011>
-- Description:	For Best Value Car
-- =======================================================================

--exec [ED_ProductLoadAllPaged] 0,7000,'0;0;0;0;0;0;0;0;0;0',0,0.0,0.0,0.0,0.0,0,0,1,0,'','','','','','','','',10000,23000,0,0,0
--exec [ED_ProductLoadAllPaged] 0,7000,'0;0;0;0;0;0;0;0;0;0',0,0.0,0.0,0.0,0.0,0,0,1,0,'','','','','','','','',0,0,0,0,0
--exec [ED_ProductLoadAllPaged] 0,7000,'0;0;0;0;0;0;0;0;0;0',0,0.0,0.0,0.0,0.0,0,0,1,0,'','','','','','','','',0,0,2006,2010,0
CREATE PROCEDURE [dbo].[ED_ProductLoadAllPaged] 
(
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@FilteredSpecs		nvarchar(MAX) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@IsBestValue		bit,
	@Lat1 float,
	@Lat2 float,
	@Long1 float,
	@Long2 float,
	@IsYear		bit,
	@IsMileage		bit,
	@IsPrice		bit,
	@CustomerID int=0,
	@CityName nvarchar(MAX),
	@Trim nvarchar(MAX),
	@Transmission nvarchar(MAX),
	@Engine nvarchar(MAX),
	@ExteriorColor nvarchar(MAX),
	@InteriorColor nvarchar(MAX),
	@DriveType nvarchar(MAX),
	@VIN nvarchar(MAX),
	@PriceMin money,
	@PriceMax money,
	@YearMin int,
	@YearMax int,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	DECLARE @Sql as nvarchar(MAX)
	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')

	If Object_Id('tempdb..#FilteredSpecs') Is Not Null 
	Drop Table #FilteredSpecs

	CREATE TABLE #FilteredSpecs
	(
		Id Bigint IDENTITY(1,1),
		AttributeOption varchar(MAX) not null
	)
	INSERT INTO #FilteredSpecs 
	SELECT data FROM [NOP_splitstring_to_table](@FilteredSpecs, ';');
	--SELECT CAST(data as int) FROM [NOP_splitstring_to_table](@FilteredSpecs, ',');

--	Select * from #FilteredSpecs
	DECLARE @MakeAttr as varchar(50)
	DECLARE @BodyAttr as varchar(50)
	DECLARE @YearAttr as varchar(50)
	DECLARE @MileageAttr as varchar(50)
	DECLARE @PriceAttr as varchar(50)
	DECLARE @Zipcode as nvarchar(max)
	DECLARE @TypeAttr as varchar(50)
	DECLARE @StateAttr as varchar(50)
	DECLARE @Model as nvarchar(Max)
	DECLARE @Flag as bit
	
	SET @MakeAttr = (Select AttributeOption From #FilteredSpecs Where Id=1)
	SET @BodyAttr = (Select AttributeOption From #FilteredSpecs Where Id=2)
	SET @YearAttr = (Select AttributeOption From #FilteredSpecs Where Id=3)
	SET @MileageAttr = (Select AttributeOption From #FilteredSpecs Where Id=4)
	SET @PriceAttr = (Select AttributeOption From #FilteredSpecs Where Id=5)
	SET @Zipcode = (Select AttributeOption From #FilteredSpecs Where Id=6)
	SET @TypeAttr = (Select AttributeOption From #FilteredSpecs Where Id=8)	
	SET @StateAttr = (Select AttributeOption From #FilteredSpecs Where Id=10)	
	SET @Model = (Select AttributeOption From #FilteredSpecs Where Id=7)
	SET @Flag=1
	

--	SELECT @MakeAttr --	SELECT @BodyAttr --	SELECT @YearAttr --	SELECT @MileageAttr --	SELECT @PriceAttr
	Declare @iDays as varchar(50)
	Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'
	print @iDays


--	SET @Sql =	'SELECT p.[ProductId],p.[Name]
--				FROM Nop_Product p Left outer join Nop_CustomerAttribute ca on p.customerid = ca. customerid
--				WHERE ca.[key] = ''ZipPostalCode'' and
--				 DATEDIFF(s,getdate(),DATEADD(d,' + @iDays + ',createdon)) > 0 And p.Deleted=0 And p.Published=1'


DECLARE @cnt as int
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
 
		--Select * from #TempOptions
 
 		select @cnt = COUNT(*)  from #TempOptions
 		
 		--Nirav - 19/07/11 - if user has selected [Select] in Miles dropdown, then Latitude1=Latitude2 and Longitude1=Longitude2. This setting is done 
 		--on SearchHome.ascx.cs. To reflect the same in SP, following lines of code is written.
 		if (@Lat1=@Lat2 and @Long1=@Long2)
 			Begin
 				Set @cnt=0
 			End
 			 		
 		Set @Flag=1
	End 

 SET @Sql =	'SELECT p.[ProductId],p.[Name]
				FROM Nop_Product p 
				left join Nop_Customer c on p.CustomerID = c.CustomerID 
				left join Nop_CustomerAttribute ca on ca.CustomerId = c.CustomerID and ca.[Key] = ''StateProvinceID''
				right join Nop_CustomerAttribute caa on caa.CustomerId = c.CustomerID and caa.[Key] = ''ZipPostalCode''
				left join Nop_CustomerAttribute cab on cab.CustomerId = c.CustomerID and cab.[Key] = ''City''
				WHERE DATEDIFF(s,getdate(),DATEADD(d,' + @iDays + ',UpdatedOn)) > 0 And p.Deleted=0 And p.Published=1 And p.QualifyPrice is not null'



	
	if(@MakeAttr != '0' And @MakeAttr != '')
	Begin
		SET @Sql = @Sql + ' And p.MakeAttribute in (SELECT data FROM 
					[NOP_splitstring_to_table](''' + @MakeAttr + ''', '',''))'
	End
	Else
	Begin
		SET @Sql = @Sql	
	End
	if(@BodyAttr != '0' And @BodyAttr != '')
	Begin
		SET @Sql = @Sql + ' And p.BodyAttribute in (SELECT data FROM 
					[NOP_splitstring_to_table](''' + @BodyAttr + ''', '',''))'
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	if(@YearAttr != '0' And @YearAttr != '')
	Begin
		SET @Sql = @Sql + ' And p.YearAttribute in (SELECT data FROM 
					[NOP_splitstring_to_table](''' + @YearAttr + ''', '',''))'
	End
	Else
	Begin
		SET @Sql = @Sql
	End 
	if(@MileageAttr != '0' And @MileageAttr != '')
	Begin
		
		If Object_Id('tempdb..#TempMileage') Is Not Null 
		Drop Table #TempMileage

		Create Table #TempMileage
		(
			Id Bigint IDENTITY(1,1),
			OptionFrom varchar(MAX),
			OptionTo varchar(MAX)
		)
		   
		Insert Into #TempMileage select AttributeOptionFrom,AttributeOptionTo from Nop_SpecificationAttributeOption where SpecificationAttributeOptionID = @MileageAttr
		Declare @MFrom as varchar(Max)
		Declare @MTo as varchar(Max)
		select @MFrom = OptionFrom , @MTo = OptionTo  from #TempMileage
		SET @Sql = @Sql + ' And p.Mileage between ' + @MFrom + ' and  ' + @MTo + ' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	if(@PriceAttr != '0' And @PriceAttr != '')
	Begin
		SET @Sql = @Sql + ' And p.PriceAttribute in (SELECT data FROM 
					[NOP_splitstring_to_table](''' + @PriceAttr + ''', '',''))'
	End
	Else
	Begin
		SET @Sql = @Sql
	End


	if(@PriceMin != 0 And @PriceMax = 0)
	Begin
		SET @Sql = @Sql + ' And p.Price_Current <= ' + Convert(Varchar, @PriceMin) + ' '
	End
	Else if(@PriceMax != 0 And @PriceMin = 0)
	Begin
		SET @Sql = @Sql + ' And p.Price_Current >= ' + Convert(Varchar, @PriceMax) + ' '
	End
	Else if(@PriceMin != 0 And @PriceMax != 0)
	Begin
		SET @Sql = @Sql + ' And p.Price_Current between ' + Convert(Varchar, @PriceMin) + ' and ' + Convert(Varchar, @PriceMax) + ''
	End
	Else
	Begin
		SET @Sql = @Sql 
	End
	
	
	if(@YearMin != 0 And @YearMax = 0 )
	Begin
		SET @Sql = @Sql + ' And p.Year <= ' + Convert(Varchar, @YearMin)+ ' '
	End
	Else if(@YearMax != 0 And @YearMin = 0)
	Begin
		SET @Sql = @Sql + ' And p.Year >= ' + Convert(Varchar, @YearMax) + ' '
	End
	Else if(@YearMin != 0 And @YearMax != 0)
	Begin
		SET @Sql = @Sql + ' And p.Year between ' + Convert(Varchar, @YearMin) + ' and ' + Convert(Varchar, @YearMax) + ''
	End
	Else
	Begin
		SET @Sql = @Sql 
	End
	
	

	if(@cnt = 0 and @Flag=1)
		Begin
			if(@Zipcode != '0' And @Zipcode != '')
				Begin
					SET @Sql = @Sql + ' And caa.[Value] in (''' + @ZipCode + ''') '
				End
			Else
				Begin
					SET @Sql = @Sql
				End
		End
	Else if(@cnt!=0 and @Flag=1)
		Begin
			SET @Sql = @Sql + ' And caa.[Value] in ( select zip_code from #TempOptions) '
		End
	Else if(@Flag=0)
		Begin
			if(@Zipcode != '0' And @Zipcode != '')
				Begin
					SET @Sql = @Sql + ' And caa.[Value] in (''' + @ZipCode + ''') '
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
	

	
	if(@TypeAttr != '0' And @TypeAttr != '')
	Begin
		SET @Sql = @Sql + ' And p.TypeAttribute in (SELECT data FROM 
					[NOP_splitstring_to_table](''' + @TypeAttr + ''', '',''))'
	End
	Else
	Begin
		SET @Sql = @Sql
	End
		
	if(@VIN != '0' And @VIN != '')
	Begin
		SET @Sql = @Sql + ' And p.VIN = ''' + @VIN + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End

	if(@StateAttr != '0' And @StateAttr != '')
	Begin
		SET @Sql = @Sql + ' And ca.[Value] = ''' + @StateAttr + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
		
	if(@CityName != '0' And @CityName != '')
	Begin
		SET @Sql = @Sql + ' And cab.[Value] = ''' + @CityName + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	if(@Trim != '0' And @Trim != '')
	Begin
		SET @Sql = @Sql + ' And p.Trim = ''' + @Trim + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	if(@Transmission != '0' And @Transmission != '')
	Begin
		SET @Sql = @Sql + ' And p.Transmission = ''' + @Transmission + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	if(@Engine != '0' And @Engine != '')
	Begin
		SET @Sql = @Sql + ' And p.Engine = ''' + @Engine + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	if(@ExteriorColor != '0' And @ExteriorColor != '')
	Begin
		SET @Sql = @Sql + ' And p.Exterior_Color = ''' + @ExteriorColor + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	
	if(@InteriorColor != '0' And @InteriorColor != '')
	Begin
		SET @Sql = @Sql + ' And p.Interior_Color = ''' + @InteriorColor + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
		if(@DriveType != '0' And @DriveType != '')
	Begin
		SET @Sql = @Sql + ' And p.Drive_Type = ''' + @DriveType + ''' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	if(@Model != '0' And @Model != '')
	Begin
		SET @Sql = @Sql + ' And p.Model in (SELECT data FROM [NOP_splitstring_to_table](''' + @Model + ''', '',''))'
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	if(@CustomerID != '0' And @CustomerID != '')
	Begin
		SET @Sql = @Sql + ' And p.CustomerID =' + Convert(Varchar,@CustomerID) + ' '
	End
	Else
	Begin
		SET @Sql = @Sql
	End
	
	
	if(@IsBestValue = 1)
	Begin
		SET @Sql = @Sql + ' ORDER BY (Price_Cost-Price_Current) DESC'
	End
	Else
	Begin
		SET @Sql = @Sql + ' ORDER BY Published'
	End	
	
	
	if(@IsYear = 1)
	Begin
		SET @Sql = @Sql + ' , p.Year ASC'
	End
	
	if(@IsMileage  = 1)
	Begin
		SET @Sql = @Sql + ' , p.Mileage ASC'
	End
	
	if(@IsPrice  = 1)
	Begin
		SET @Sql = @Sql + ' , p.Price_Current ASC'
	End
	
	
	PRINT(@Sql)
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
--	Select @PageLowerBound
--	Select @PageUpperBound
	CREATE TABLE #DisplayOrderTmp 
	(
		[ID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL,
		[Name] nvarchar(400) not null
	)

	INSERT INTO #DisplayOrderTmp
	EXEC(@Sql)
	--Select * from #DisplayOrderTmp

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
	--Select * from #PageIndex

	--total records
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
			p.[FileName],p.[IsNew],p.[IsFeature],p.[SavingAmount],p.[City],p.[StateID] ,p.[QualifyPrice] ,p.[OwnerDetail],p.[Show_on_Dealer],p.[Offer],
			p.[City_Fuel],p.[Highway_Fuel],p.[AverageRetailPrice],p.[AverageTradeinPrice],
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

	DROP TABLE #FilteredSpecs
	DROP TABLE #DisplayOrderTmp
	DROP TABLE #PageIndex
END