-- =============================================
-- Author:		<Manali Panchal>
-- Created on: <12/3/2011>
-- Description:	Insert Product And Product Variant Main
-- Updated on: <16/3/2011>
-- Description:	For inserting product specification mapping
-- =============================================
--exec [dbo].[ED_ProductAndVariantInsert] '1G2NE12T31M558427'
CREATE PROCEDURE [dbo].[ED_ProductAndVariantInsert] --'1G2NE12T31M558427'
	@VIN varchar(MAX),
	@PrdId bigint OUTPUT
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int
	DECLARE @ProductID int
	Declare @Options varchar(max)
	
	--Insert Product
	INSERT INTO [Nop_Product]
	(
		[Name],ShortDescription,FullDescription,AdminComment,ProductTypeID,
		TemplateID,ShowOnHomePage,MetaKeywords,MetaDescription,MetaTitle,
		SEName,AllowCustomerReviews,AllowCustomerRatings,RatingSum,
		TotalRatingVotes,Published,Deleted,CreatedOn,UpdatedOn,VIN,CustomerID,
		VehicleName,[Type],TypeAttribute,Stock,[Year],YearAttribute,Make,MakeAttribute,
		Model,Trim,Free_Text,Body,BodyAttribute,Mileage,MileageAttribute,
		Price_Current,Reserved,Price_Wholesale,Price_Cost,PriceAttribute,
		Title,Condition,Exterior_Color,Interior_Color,Doors,Engine,
		Transmission,Fuel_Type,Drive_Type,Options,Warranty,WarrantyAttribute,Description,
		Pics,Dealer_Name,Dealer_Zip,City,StateID,Date_in_Stock,[FileName],IsNew,IsFeature,QualifyPrice,
		OwnerDetail,Show_on_Dealer,Offer,City_Fuel,Highway_Fuel,AverageRetailPrice,AverageTradeinPrice
	)
	VALUES
	(
		'Title','','','',1,5,'false','','','','','true','true',0,0,'true',
		'false',getdate(),getdate(),@VIN,0,'','',0,'','',0,'',0,'','','','',0,
		'',0,0,'',0,0,0,'','','','','','','','','','','',0,
		'','','','','',0,getdate(),'','true','false',0,
		'','false','false',0,0,0,0
	)				
	set @ProductID=SCOPE_IDENTITY()
	Set @PrdId=SCOPE_IDENTITY()
	
	
	Update Nop_Product
	Set
		[Name]=EP.VehicleName,
		VIN=EP.VIN,
		CustomerID=EP.CustomerID,
		VehicleName=EP.VehicleName,
		[Type]=EP.[Type],
		TypeAttribute=EP.TypeAttribute,
		Stock=EP.Stock,
		[Year]=EP.[Year],
		YearAttribute=EP.YearAttribute,
		Make=EP.Make,
		MakeAttribute=EP.MakeAttribute,
		Model=EP.Model,
		Trim=EP.Trim,
		Free_Text=EP.Free_Text,
		Body=EP.Body,
		BodyAttribute=EP.BodyAttribute,
		Mileage=EP.Mileage,
		MileageAttribute=EP.MileageAttribute,
		Price_Current=EP.Price_Current,
		Reserved=EP.Reserved,
		Price_Wholesale=EP.Price_Wholesale,
		Price_Cost=EP.Price_Cost,
		PriceAttribute=EP.PriceAttribute,
		Title=EP.Title,
		Condition=EP.Condition,
		Exterior_Color=EP.Exterior_Color,
		Interior_Color=EP.Interior_Color,
		Doors=EP.Doors,
		Engine=EP.Engine,
		Transmission=EP.Transmission,
		Fuel_Type=EP.Fuel_Type,
		Drive_Type=EP.Drive_Type,
		Options=EP.Options,
		Warranty=EP.Warranty,
		WarrantyAttribute=EP.WarrantyAttribute,
		Description=EP.Description,
		Pics=EP.Pics,
		Dealer_Name=EP.Dealer_Name,
		Dealer_Zip=EP.Dealer_Zip,
		City=EP.City,
		StateID=EP.StateID,
		Date_in_Stock=EP.Date_in_Stock,
		[FileName]=EP.[FileName],
		UpdatedOn = Getdate(),
		City_Fuel=EP.City_Fuel,
		Highway_Fuel=EP.Highway_Fuel,
		AverageRetailPrice=EP.AverageRetailPrice,
		AverageTradeinPrice=EP.AverageTradeinPrice
	from Nop_Product as Product 
	LEFT JOIN ED_ProductData as EP On EP.VIN = Product.VIN 
	Where Product.Deleted = 0 And Product.ProductID=@ProductID
	
	-- Insert Product Variant
	INSERT INTO [Nop_ProductVariant]
    (
        ProductId,[Name],SKU,[Description],AdminComment,ManufacturerPartNumber,
		IsGiftCard,IsDownload,DownloadID,UnlimitedDownloads,MaxNumberOfDownloads,
		DownloadExpirationDays,DownloadActivationType,HasSampleDownload,
		SampleDownloadID,HasUserAgreement,UserAgreementText,IsRecurring,
		CycleLength,CyclePeriod,TotalCycles,IsShipEnabled,IsFreeShipping,
		AdditionalShippingCharge,IsTaxExempt,TaxCategoryID,ManageInventory,
		DisplayStockAvailability,StockQuantity,MinStockQuantity,
        LowStockActivityID,NotifyAdminForQuantityBelow,AllowOutOfStockOrders,
		OrderMinimumQuantity,OrderMaximumQuantity,WarehouseId,DisableBuyButton,
        Price,OldPrice,ProductCost,CustomerEntersPrice,MinimumCustomerEnteredPrice,
		MaximumCustomerEnteredPrice,Weight,[Length],Width,Height,PictureID,
		AvailableStartDateTime,AvailableEndDateTime,Published,Deleted,
        DisplayOrder,CreatedOn,UpdatedOn
    )
    VALUES
    (
        @ProductID,'','','','','','false','false',0,'false',10000,0,1,
		'false',0,'false','','false',0,0,0,'false','false',0,'false',0,
		0,'false',0,0,0,0,'false',0,99999,0,'false',0,0,0,'false',0,0,0,0,0,0,0,null,
		null,'true','false',1,getdate(),getdate()
    )
    
	If Object_Id('tempdb..#TempOptions') Is Not Null 
	Drop Table #TempOptions

	Create Table #TempOptions
	(
		Id Bigint IDENTITY(1,1),
		OptionName varchar(MAX),
		OptionId int NULL
	)    
	
	--Nirav - 28-Jun-2011
	--To insert options in ED_VehicleOptions
	Select @Options = Options from Nop_Product Where ProductId=@ProductID

	
	Insert Into #TempOptions
	Select data as OptionName,Null From dbo.NOP_splitstring_to_table(@Options,';')
		
	
	Insert Into [ED_VehicleOptions] (VehicleOptionName,DisplayOrder)
	Select RTrim(LTrim(OptionName)),1 from #TempOptions 
	Where RTrim(LTRIM(OptionName)) Not In
	(Select VehicleOptionName from ED_VehicleOptions)
    
    update #TempOptions set OptionId = B.VehicleOptionId  from #TempOptions 
    left join ED_VehicleOptions as B On OptionName = b.VehicleOptionName
       
   -- select * from #TempOptions
    
	Declare @Count As Bigint
	Declare @cnt as bigint
	Declare @TempStr as varchar(MAX)
	Declare @Temp As Bigint
	
	Set @cnt=0
	Select @Count=Max(Id) From #TempOptions
	Print(@Count)
	
	--While (Select @cnt)< @Count
	--Begin
	--	Set @cnt = @cnt + 1
	--	Set @Temp = (Select OptionId From #TempOptions
	--					Where Id=@cnt)
		
	--	Set @TempStr = @TempStr + ',' + @Temp
	--	Print (@TempStr)
	--End
		 
	SELECT @TempStr = coalesce(@TempStr+',','') + CONVERT(varchar, OptionId)
	FROM #TempOptions
	
	Print (@TempStr)
	
	update Nop_Product set Options =@TempStr 
	where ProductId = @ProductID
    
   -- select * from Nop_Product
    
	EXEC [dbo].[ED_ProductSpecMappingInsert] @ProductID
END