-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <18/2/2011>
-- Description:	Insert Product Data
-- Updated on: <14/3/2011>
-- Description: For adding Specification Attribute
-- ================================================
CREATE PROCEDURE [dbo].[ED_ProductDataInsert] --'33585700',38,'Used 2011 Audi','Audi','Car',
--				'Audi','Audi','Audi','Audi','Audi','Audi','Audi','Audi',
--				'Audi','Audi','Mercedes-Benz','Audi','Audi','','17424','2011',
--				'Audi',18000,'Audi1','Audi1','Audi1','Audi1','Audi1','Audi1','Audi1',
--				'Audi1','Audi1','1/1/1900','1/1/1900','1/1/1900','Audi1'
(
	@VIN varchar(100),
	@CustomerID int,
	@VehicleName varchar(MAX),
	@Type varchar(MAX),
	@Stock varchar(MAX),
	@Year varchar(MAX),
	@Make varchar(MAX),
	@Model varchar(MAX),
	@Trim varchar(MAX),
	@Free_Text varchar(MAX),
	@Body varchar(MAX),
	@Mileage varchar(MAX),
	@Price_Current money,
	@Reserved varchar(MAX),
	@Price_Wholesale money,
	@Price_Cost money,
	@Title varchar(MAX),
	@Condition varchar(MAX),
	@Exterior_Color varchar(MAX),
	@Interior_Color varchar(MAX),
	@Doors varchar(MAX),
	@Engine varchar(MAX),
	@Transmission varchar(MAX),
	@Fuel_Type varchar(MAX),
	@Drive_Type varchar(MAX),
	@Options varchar(MAX),
	@Warranty varchar(MAX),
	@Description varchar(MAX),
	@Pics varchar(MAX),
	@Date_in_Stock varchar(MAX),
	@CreatedOn datetime,
	@FileName varchar(MAX),
	@DealerName varchar(Max),
	@DealerZip varchar(Max),
	@City varchar(Max),
	@StateID int,
	@ValidPrice bit,
	@City_Fuel int,
	@Highway_Fuel int,
	@Retail_Price money,
	@Tradein_Price money,
	@ProductExists bit output
)
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int
	DECLARE @ParaId int
	DECLARE @Temp int
	--Region Get Specification Attribute Option Id
	DECLARE @OptionsTemp varchar(MAX)
	DECLARE @TempMake int
	DECLARE @TempBody int
	DECLARE @TempYear int
	DECLARE @TempType int
	DECLARE @TempWarranty int
	DECLARE @TempMileage int
	DECLARE @TempPrice int
	SET @OptionsTemp = @Make + ',' + @Body + ',' + @Year + ',' + @Mileage + ',' + Convert(varchar(50),@Price_Current) + ',' + '0' + ',' + '0' + ',' + @Type + ',' +@Warranty
	--Select @Options
	If Object_Id('tempdb..#TempSpec') Is Not Null 
	Drop Table #TempSpec

	Create Table #TempSpec
	(
		Id Bigint IDENTITY(1,1),
		List varchar(MAX)
	)

	Insert Into #TempSpec
	EXEC [dbo].[ED_GetSpecificationAttrOptionId] @OptionsTemp
	
	SET @TempMake = (Select List From #TempSpec Where Id=1) --1st Row : Make
	SET @TempBody = (Select List From #TempSpec Where Id=2) --2nd Row : Body
	SET @TempYear = (Select List From #TempSpec Where Id=3) --3rd Row : Year
	SET @TempMileage = (Select List From #TempSpec Where Id=4) --4th Row : Mileage
	SET @TempType = (Select List From #TempSpec Where Id=8) --4th Row : Mileage
	SET @TempWarranty = (Select List From #TempSpec Where Id=9) --4th Row : Mileage
	SET @TempPrice = (Select List From #TempSpec Where Id=5) --5th Row : Price
	--End Region
--	Select @TempMake 
--	Select @TempBody
--	Select @TempYear
--	Select @TempMileage
--	Select @TempPrice

	--Region Check Whether Product is New or Existing one
	SET @Temp = (Select count(ProductId) From Nop_Product Where Deleted=0 And VIN=@VIN)
	if(@Temp > 0)
		Begin
			SET @ParaId=1 --Existing Product
			Set @ProductExists=1
		End
	Else
		Begin
			SET @ParaId=3 --New Product
			Set @ProductExists=0
		End
	--End Region

	--If Price is not valid as compared to NADA, then mark existing product as deleted
	if(@ValidPrice = 0)
	Begin
		if (@ParaId = 1)
		Begin
			Update Nop_Product Set Deleted=1 Where VIN=@VIN
		End
	End
	
	if(@ParaId = 3 and @ValidPrice=1) --Insert record only if price is valid compared to NADA
	Begin
		--New Product
		INSERT
		INTO [ED_ProductData]
		(
			VIN,
			CustomerID,
			VehicleName,
			[Type],
			TypeAttribute,
			Stock,
			[Year],
			YearAttribute,
			Make,
			MakeAttribute,
			Model,
			Trim,
			Free_Text,
			Body,
			BodyAttribute,
			Mileage,
			MileageAttribute,
			Price_Current,
			Reserved,
			Price_Wholesale,
			Price_Cost,
			PriceAttribute,
			Title,
			Condition,
			Exterior_Color,
			Interior_Color,
			Doors,
			Engine,
			Transmission,
			Fuel_Type,
			Drive_Type,
			Options,
			Warranty,
			WarrantyAttribute,
			Description,
			Pics,
			Dealer_Name,
			Dealer_Zip,
			City,
			StateID,
			Date_in_Stock,
			CreatedOn,
			ParaId,
			[FileName],
			City_Fuel,
			Highway_Fuel,
			AverageRetailPrice,
			AverageTradeinPrice
		)
		VALUES
		(
			@VIN,
			@CustomerID,
			@VehicleName,
			@Type,
			@TempType,
			@Stock,
			@Year,
			@TempYear,
			@Make,
			@TempMake,
			@Model,
			@Trim,
			@Free_Text,
			@Body,
			@TempBody,
			@Mileage,
			@TempMileage,
			@Price_Current,
			@Reserved,
			@Price_Wholesale,
			@Price_Cost,
			@TempPrice,
			@Title,
			@Condition,
			@Exterior_Color,
			@Interior_Color,
			@Doors,
			@Engine,
			@Transmission,
			@Fuel_Type,
			@Drive_Type,
			@Options,
			@Warranty,
			@TempWarranty,
			@Description,
			@Pics,
			@DealerName,
			@DealerZip,
			@City,
			@StateID,
			@Date_in_Stock,
			@CreatedOn,
			@ParaId,
			@FileName,
			@City_Fuel,
			@Highway_Fuel ,
			@Retail_Price ,
			@Tradein_Price 
		)

		SET @Err = @@Error	
		
		 
	
		
	End
	Else if(@ParaId = 1 and @ValidPrice=1) --Update record if Price is valid compared to NADA
	Begin
		--Existing Product
		
		--Added by henisha rathod
		
		If Object_Id('tempdb..#TempOptions') Is Not Null 
		Drop Table #TempOptions

		Create Table #TempOptions
		(
			Id Bigint IDENTITY(1,1),
			OptionName varchar(MAX),
			OptionId int NULL
		)  
		
		Insert Into #TempOptions
		Select data as OptionName,Null From dbo.NOP_splitstring_to_table(@Options,';')
		
		Insert Into [ED_VehicleOptions] (VehicleOptionName,DisplayOrder)
		Select RTrim(LTrim(OptionName)),1 from #TempOptions 
		Where RTrim(LTRIM(OptionName)) Not In
		(Select VehicleOptionName from ED_VehicleOptions)
    
		update #TempOptions set OptionId = B.VehicleOptionId  from #TempOptions 
		left join ED_VehicleOptions as B On OptionName = b.VehicleOptionName
       
		-- select * from #TempOptions
    
		Declare @TempStr as varchar(MAX)
		 
		SELECT @TempStr = coalesce(@TempStr+',','') + CONVERT(varchar, OptionId)
		FROM #TempOptions
	
		--Print (@TempStr)
		
		
		Update Nop_Product
		Set
			[Name]=@VehicleName,
			VIN=@VIN,
			CustomerID=@CustomerID,
			VehicleName=@VehicleName,
			[Type]=@Type,
			TypeAttribute=@TempType,
			Stock=@Stock,
			[Year]=@Year,
			YearAttribute=@TempYear,
			Make=@Make,
			MakeAttribute=@TempMake,
			Model=@Model,
			Trim=@Trim,
			Free_Text=@Free_Text,
			Body=@Body,
			BodyAttribute=@TempBody,
			Mileage=@Mileage,
			MileageAttribute=@TempMileage,
			Price_Current=@Price_Current,
			Reserved=@Reserved,
			Price_Wholesale=@Price_Wholesale,
			Price_Cost=@Price_Cost,
			PriceAttribute=@TempPrice,
			Title=@Title,
			Condition=@Condition,
			Exterior_Color=@Exterior_Color,
			Interior_Color=@Interior_Color,
			Doors=@Doors,
			Engine=@Engine,
			Transmission=@Transmission,
			Fuel_Type=@Fuel_Type,
			Drive_Type=@Drive_Type,
			Options=@TempStr,
			Warranty=@Warranty,
			WarrantyAttribute=@TempWarranty,
			Description=@Description,
			Pics=@Pics,
			Dealer_Name=@DealerName,
			Dealer_Zip=@DealerZip,
			City = @City,
			StateID =@StateID,
			Date_in_Stock=@Date_in_Stock,
			[FileName]=@FileName,
			IsNew=0,
			City_Fuel =@City_Fuel,
			Highway_Fuel =@Highway_Fuel,
			AverageRetailPrice =@Retail_Price,
			AverageTradeinPrice =@Tradein_Price,
			UpdatedOn = Getdate()
			
		from Nop_Product
		Where VIN=@VIN And Deleted = 0 
		
		--Update Product Specification Attribute Mapping
		if (@ValidPrice=1)
		Begin
			Declare @Attribute varchar(MAX)
			Set @Attribute = convert(varchar(50),@TempMake) + ',' +
							 convert(varchar(50),@TempBody) + ',' + 
							 convert(varchar(50),@TempYear) + ',' +
							 convert(varchar(50),@TempMileage) + ',' +
							 convert(varchar(50),@TempPrice)+','+convert(varchar(50),@TempType)
							 +','+convert(varchar(50),@TempWarranty)
			EXEC [dbo].[ED_UpdateProductSpecByAttribute] @VIN,@Attribute
		End
	End
	
	RETURN @Err
END