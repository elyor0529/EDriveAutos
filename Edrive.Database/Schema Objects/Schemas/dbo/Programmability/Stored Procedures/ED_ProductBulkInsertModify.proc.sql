-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <14/2/2011>
-- Description:	Bulk product insert
-- Updated Date: <29/7/2011>
-- For Product Specification Attribute Mapping section
-- =============================================

-- EXEC [dbo].[ED_ProductBulkInsertModify] 'D:\Nyusoft\Development\EDrive\VEHICLES.CSV','37'
CREATE PROCEDURE [dbo].[ED_ProductBulkInsertModify] --'D:\Nyusoft\Development\EDrive\Nirav_TestTwo.CSV','37'
	@File nvarchar(500),
	@CustomerID int
AS
BEGIN

Declare @ParaList as nvarchar(500)
Declare @Sql as nvarchar(1000)
Select @Paralist = '@File nvarchar(500)' 

	/************ FOR PRODUCT ************/

	If Object_Id('tempdb..#Temp') Is Not Null 
	Drop Table #Temp

	Create Table #Temp
	(
		[1] nvarchar(max),
		[2] nvarchar(max),
		[3] nvarchar(max),
		[4] nvarchar(max),
		[5] nvarchar(max),
		[6] nvarchar(max),
		[7] nvarchar(max),
		[8] nvarchar(max),
		[9] nvarchar(max),
		[10] nvarchar(max),
		[11] nvarchar(max),
		[12] nvarchar(max),
		[13] nvarchar(max),
		[14] nvarchar(max),
		[15] nvarchar(max),
		[16] nvarchar(max),
		[17] nvarchar(max),
		[18] nvarchar(max),
		[19] nvarchar(max),
		[20] nvarchar(max),
		[21] nvarchar(max),
		[22] nvarchar(max),
		[23] nvarchar(max),
		[24] nvarchar(max),
		[25] nvarchar(max),
		[26] nvarchar(max),
		[27] nvarchar(max),
		[28] nvarchar(max),
		[29] nvarchar(max),
		[30] nvarchar(max),
		[31] nvarchar(max),
		[32] nvarchar(max)
		
	)

	--Select @Sql = 'BULK INSERT #Temp FROM "' + @File + '"
	--WITH
	--(
	--  FIRSTROW = 2
	-- ,FIELDTERMINATOR = '',''
	--)'

	--Exec sp_executesql @Sql,@ParaList,@File
	
	
	--select * from #Temp
	
	Select @Sql = 'Insert into #Temp([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],
				[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],
				[24],[25],[26],[27],[28],[29],[30],[31],[32])
	SELECT * FROM
	OPENROWSET(''MSDASQL'',''Driver={Microsoft Text Driver (*.txt; *.csv)};
	DefaultDir=D:\Nyusoft\Development\EDrive'',
	''SELECT * FROM ' + @File + '''
	)'

	Exec sp_executesql @Sql,@ParaList,@File
		
	--select * from #Temp	

	Update #Temp
	SET
	[1] = CASE 
				WHEN LEFT([1],1) = '"' THEN Rtrim(Ltrim(Substring([1], 2, LEN([1]) - 2)))
				ELSE [1] 
		  END,
	[2] = CASE 
				WHEN LEFT([2],1) = '"' THEN Rtrim(Ltrim(Substring([2], 2, LEN([2]) - 2)))
				ELSE [2] 
		  END,
	[3] = CASE 
				WHEN LEFT([3],1) = '"' THEN Rtrim(Ltrim(Substring([3], 2, LEN([3]) - 2)))
				ELSE [3] 
		  END,
	[4] = CASE 
				WHEN LEFT([4],1) = '"' THEN Rtrim(Ltrim(Substring([4], 2, LEN([4]) - 2)))
				ELSE [4] 
		  END,
	[5] = CASE 
				WHEN LEFT([5],1) = '"' THEN Rtrim(Ltrim(Substring([5], 2, LEN([5]) - 2)))
				ELSE [5] 
		  END,
	[6] = CASE 
				WHEN LEFT([6],1) = '"' THEN Rtrim(Ltrim(Substring([6], 2, LEN([6]) - 2)))
				ELSE [6] 
		  END,
	[7] = CASE 
				WHEN LEFT([7],1) = '"' THEN Rtrim(Ltrim(Substring([7], 2, LEN([7]) - 2)))
				ELSE [7] 
		  END,
	[8] = CASE 
				WHEN LEFT([8],1) = '"' THEN Rtrim(Ltrim(Substring([8], 2, LEN([8]) - 2)))
				ELSE [8] 
		  END,
	[9] = CASE 
				WHEN LEFT([9],1) = '"' THEN Rtrim(Ltrim(Substring([9], 2, LEN([9]) - 2)))
				ELSE [9] 
		  END,
	[10] = CASE 
				WHEN LEFT([10],1) = '"' THEN Rtrim(Ltrim(Substring([10], 2, LEN([10]) - 2)))
				ELSE [10] 
		  END,
	[11] = CASE 
				WHEN LEFT([11],1) = '"' THEN Rtrim(Ltrim(Substring([11], 2, LEN([11]) - 2)))
				ELSE [11] 
		  END,
	[12] = CASE 
				WHEN LEFT([12],1) = '"' THEN Rtrim(Ltrim(Substring([12], 2, LEN([12]) - 2)))
				ELSE [12] 
		  END,
	[13] = CASE 
				WHEN LEFT([13],1) = '"' THEN Rtrim(Ltrim(Substring([13], 2, LEN([13]) - 2)))
				ELSE [13] 
		  END,
	[14] = CASE 
				WHEN LEFT([14],1) = '"' THEN Rtrim(Ltrim(Substring([14], 2, LEN([14]) - 2)))
				ELSE [14] 
		  END,
	[15] = CASE 
				WHEN LEFT([15],1) = '"' THEN Rtrim(Ltrim(Substring([15], 2, LEN([15]) - 2)))
				ELSE [15] 
		  END,
	[16] = CASE 
				WHEN LEFT([16],1) = '"' THEN Rtrim(Ltrim(Substring([16], 2, LEN([16]) - 2)))
				ELSE [16] 
		  END,
	[17] = CASE 
				WHEN LEFT([17],1) = '"' THEN Rtrim(Ltrim(Substring([17], 2, LEN([17]) - 2)))
				ELSE [17] 
		  END,
	[18] = CASE 
				WHEN LEFT([18],1) = '"' THEN Rtrim(Ltrim(Substring([18], 2, LEN([18]) - 2)))
				ELSE [18] 
		  END,	
	[19] = CASE 
				WHEN LEFT([19],1) = '"' THEN Rtrim(Ltrim(Substring([19], 2, LEN([19]) - 2)))
				ELSE [19] 
		  END,
	[20] = CASE 
				WHEN LEFT([20],1) = '"' THEN Rtrim(Ltrim(Substring([20], 2, LEN([20]) - 2)))
				ELSE [20] 
		  END,
	[21] = CASE 
				WHEN LEFT([21],1) = '"' THEN Rtrim(Ltrim(Substring([21], 2, LEN([21]) - 2)))
				ELSE [21] 
		  END,
	[23] = CASE 
				WHEN LEFT([23],1) = '"' THEN Rtrim(Ltrim(Substring([23], 2, LEN([23]) - 2)))
				ELSE [23] 
		  END,
	[24] = CASE 
				WHEN LEFT([24],1) = '"' THEN Rtrim(Ltrim(Substring([24], 2, LEN([24]) - 2)))
				ELSE [24] 
		  END,
	[25] = CASE 
				WHEN LEFT([25],1) = '"' THEN Rtrim(Ltrim(Substring([25], 2, LEN([25]) - 2)))
				ELSE [25] 
		  END,
	[26] = CASE 
				WHEN LEFT([26],1) = '"' THEN Rtrim(Ltrim(Substring([26], 2, LEN([26]) - 2)))
				ELSE [26] 
		  END,
	[27] = CASE 
				WHEN LEFT([27],1) = '"' THEN Rtrim(Ltrim(Substring([27], 2, LEN([27]) - 2)))
				ELSE [27] 
		  END
		,
	[29] = CASE 
				WHEN LEFT([29],1) = '"' THEN Rtrim(Ltrim(Substring([29], 2, LEN([29]) - 2)))
				ELSE [29] 
	END
		,
	[30] = CASE 
				WHEN LEFT([30],1) = '"' THEN Rtrim(Ltrim(Substring([30], 2, LEN([30]) - 2)))
				ELSE [30] 
	END	
		,
	[31] = CASE 
				WHEN LEFT([31],1) = '"' THEN Rtrim(Ltrim(Substring([31], 2, LEN([31]) - 2)))
				ELSE [31] 
	END	
		
		,
	[32] = CASE 
				WHEN LEFT([32],1) = '"' THEN Rtrim(Ltrim(Substring([32], 2, LEN([32]) - 2)))
				ELSE [32] 
	END
	
	--select * from #Temp
		
	Update #Temp
	SET
	[28] = CASE 
				WHEN LEFT([28],1) = '"' THEN Rtrim(Ltrim(Substring([28], 2, LEN([28]) - 2)))
				ELSE [28] 
		  END
	WHERE [28] is not null

	Declare @StateNo As int
	DECLARE @ProductID int
	DECLARE @EDProductID int
	 	 
	set @StateNo = 0
			
	Insert into ED_JunkProductData
	Select [3],@CustomerID,[4]+' '+[5]+' '+[6],[1],[2],[4],[5],[6],[7],
			[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],
			[22],[23],[24],[25],[26],[27],[28],getdate(),3,'',[29],[30],[31],@StateNo
	From #Temp Where [3] is null Or [11] is null Or [11] = 0
	
	DELETE from #Temp Where [3] is null Or [11] is null Or [11] = 0
	
	EXEC [dbo].[ED_DeleteProductDataByCustomerID] @CustomerID
	
	--Region Get Specification Attribute Option Id
	If Object_Id('tempdb..#TempMakeAttr') Is Not Null 
	Drop Table #TempMakeAttr

	Create Table #TempMakeAttr
	(
		Id1 Bigint IDENTITY(1,1),
		MakeList varchar(MAX)
	)

	Insert Into #TempMakeAttr
	Select Distinct([5]) From #Temp Where [5] Not In 
			(Select [Name] From Nop_SpecificationAttributeOption 
			Where SpecificationAttributeID=1)
	
	Declare @Count1 As Bigint
	Declare @cnt1 as bigint
	Declare @Temp1 as varchar(MAX)
	Set @cnt1=0
	Select @Count1=Max(Id1) From #TempMakeAttr

	While (Select @cnt1)<@count1
	Begin
		Set @cnt1 = @cnt1 + 1
		Select @Temp1= MakeList from #TempMakeAttr where Id1=@cnt1
		Set @Temp1 = LTrim(RTrim(@Temp1))
		EXEC [dbo].[Nop_SpecificationAttributeOptionInsert] null,1,@Temp1,1,0,0
	End

	-- drop make virtual table
	
	If Object_Id('tempdb..#TempMakeAttr') Is Not Null 
	Drop Table #TempMakeAttr

	
	If Object_Id('tempdb..#TempBodyAttr') Is Not Null 
	Drop Table #TempBodyAttr

	Create Table #TempBodyAttr
	(
		Id2 Bigint IDENTITY(1,1),
		BodyList varchar(MAX)
	)

	Insert Into #TempBodyAttr
	Select Distinct([9]) From #Temp Where [9] Not In 
			(Select [Name] From Nop_SpecificationAttributeOption 
			Where SpecificationAttributeID=2)
	
	Declare @Count2 As Bigint
	Declare @cnt2 as bigint
	Declare @Temp2 as varchar(MAX)
	Set @cnt2=0
	Select @Count2=Max(Id2) From #TempBodyAttr

	While (Select @cnt2)<@count2
	Begin
		Set @cnt2 = @cnt2 + 1
		Select @Temp2= BodyList from #TempBodyAttr where Id2=@cnt2
		Set @Temp2 = LTrim(RTrim(@Temp2))
		EXEC [dbo].[Nop_SpecificationAttributeOptionInsert] null,2,@Temp2,1,0,0
	End
		
	-- drop body virtual table
	
	If Object_Id('tempdb..#TempBodyAttr') Is Not Null 
	Drop Table #TempBodyAttr	
		
	If Object_Id('tempdb..#TempYearAttr') Is Not Null 
	Drop Table #TempYearAttr

	Create Table #TempYearAttr
	(
		Id3 Bigint IDENTITY(1,1),
		YearList varchar(MAX)
	)

	Insert Into #TempYearAttr
	Select Distinct([4]) From #Temp Where [4] Not In 
			(Select [Name] From Nop_SpecificationAttributeOption 
			Where SpecificationAttributeID=3)
	
	Declare @Count3 As Bigint
	Declare @cnt3 as bigint
	Declare @Temp3 as varchar(MAX)
	Set @cnt3=0
	Select @Count3=Max(Id3) From #TempYearAttr

	While (Select @cnt3)<@count3
	Begin
		Set @cnt3 = @cnt3 + 1
		Select @Temp3= YearList from #TempYearAttr where Id3=@cnt3
		Set @Temp3 = LTrim(RTrim(@Temp3))
		EXEC [dbo].[Nop_SpecificationAttributeOptionInsert] null,3,@Temp3,1,0,0
	End
	
	-- drop year virtual table
	
	If Object_Id('tempdb..#TempYearAttr') Is Not Null 
	Drop Table #TempYearAttr	
	
	---- for Type attribute
	
	
	If Object_Id('tempdb..#TempTypeAttr') Is Not Null 
	Drop Table #TempTypeAttr

	Create Table #TempTypeAttr
	(
		Id4 Bigint IDENTITY(1,1),
		TypeList varchar(MAX)
	)

	Insert Into #TempTypeAttr
	Select Distinct([4]) From #Temp Where [4] Not In 
			(Select [Name] From Nop_SpecificationAttributeOption 
			Where SpecificationAttributeID=8)
	
	Declare @Count4 As Bigint
	Declare @cnt4 as bigint
	Declare @Temp4 as varchar(MAX)
	Set @cnt4=0
	Select @Count4=Max(Id4) From #TempTypeAttr

	While (Select @cnt4)<@count4
	Begin
		Set @cnt4 = @cnt4 + 1
		Select @Temp4= TypeList from #TempTypeAttr where Id4=@cnt4
		Set @Temp4 = LTrim(RTrim(@Temp4))
		EXEC [dbo].[Nop_SpecificationAttributeOptionInsert] null,2,@Temp4,1,0,0
	End
	
	-- drop type virtual table
		
	If Object_Id('tempdb..#TempTypeAttr') Is Not Null 
	Drop Table #TempTypeAttr		
	
	---- for Warranty attribute
		
	If Object_Id('tempdb..#TempWarrantyAttr') Is Not Null 
	Drop Table #TempWarrantyAttr

	Create Table #TempWarrantyAttr
	(
		Id5 Bigint IDENTITY(1,1),
		WarrantyList varchar(MAX)
	)

	Insert Into #TempWarrantyAttr
	Select Distinct([4]) From #Temp Where [4] Not In 
			(Select [Name] From Nop_SpecificationAttributeOption 
			Where SpecificationAttributeID=9)
	
	Declare @Count5 As Bigint
	Declare @cnt5 as bigint
	Declare @Temp5 as varchar(MAX)
	Set @cnt5=0
	Select @Count5=Max(Id5) From #TempWarrantyAttr

	While (Select @cnt5)<@count5
	Begin
		Set @cnt5 = @cnt5 + 1
		Select @Temp5= WarrantyList from #TempWarrantyAttr where Id5=@cnt5
		Set @Temp5 = LTrim(RTrim(@Temp5))
		EXEC [dbo].[Nop_SpecificationAttributeOptionInsert] null,2,@Temp5,1,0,0
	End
	
	-- drop warranty virtual table
	
	If Object_Id('tempdb..#TempWarrantyAttr') Is Not Null
	Drop Table #TempWarrantyAttr		

---- for option attribute
    
 --   If Object_Id('tempdb..#TempOption') Is Not Null 
	--Drop Table #TempOption

	--Create Table #TempOption
	--(
	--	Id11 Bigint IDENTITY(1,1),
	--	OptionList varchar(MAX)
	--)

	--Insert Into #TempOption
	--Select Distinct([24]) From #Temp 
		
	--Declare @Count11 As Bigint
	--Declare @cnt11 as bigint
	--Declare @Temp11 as varchar(MAX)
	--Set @cnt11=0
	--Select @Count11=Max(Id11) From #TempOption

	--While (Select @cnt11)<@count11
	--Begin
	--	Set @cnt11 = @cnt11 + 1
	--	Select @Temp11= OptionList from #TempOption where Id11=@cnt11
	--	Set @Temp11 = LTrim(RTrim(@Temp11))
	--	EXEC dbo.Nop_VehicleOptionBulkInsert @Temp11
	--End
		
	-- drop Option virtual table
	
	--If Object_Id('tempdb..#TempOption') Is Not Null
	--Drop Table #TempOption	

	
	--Insert into Nop_Product
	--Select 'Title','','','',1,5,'false','','','','','true','true',0,0,'true',
	--	'false',getdate(),getdate(),[3],0,'','',0,'','',0,'',0,'','','','',0,
	--	'',0,0,'',0,0,0,'','','','','','','','','','','',0,
	--	'','',getdate(),'','true','false','','',0,'',0
	--From #Temp Where [3] is not null Or [11] is not null Or [11] != 0
	
	--set @ProductID=SCOPE_IDENTITY()
	
	
	
	---- for Product Variant attribute
		
	--If Object_Id('tempdb..#TempProductVariant') Is Not Null 
	--Drop Table #TempProductVariant

	--Create Table #TempProductVariant
	--(
	--	Id7 Bigint IDENTITY(1,1),
	--	ProductVariantList varchar(MAX)
	--)

	--Insert Into #TempProductVariant
	--Select Distinct([3]) From #Temp 
	
	--Declare @Count7 As Bigint
	--Declare @cnt7 as bigint
	--Declare @Temp7 as varchar(MAX)
	--Set @cnt7=0
	--Select @Count7=Max(Id7) From #TempProductVariant

	--While (Select @cnt7)<@count7
	--Begin
	--	Set @cnt7 = @cnt7 + 1
	--	Select @Temp7= ProductVariantList from #TempProductVariant where Id7=@cnt7
	--	Set @Temp7 = LTrim(RTrim(@Temp7))
	--	EXEC dbo.Nop_ProductVariantInsertNew NULL,@Temp7
	--End
		
	--If Object_Id('tempdb..#TempProductVariant') Is Not Null 
	--Drop Table #TempProductVariant		

	
	--DECLARE @count INT
	--SELECT  @count = COUNT(*) FROM #Temp
 --   WHILE (@count > 0) 
 --   BEGIN
	--Select [3] as [3] from #Temp Where [3] is not null Or [11] is not null Or [11] != 0
	--EXEC dbo.Nop_ProductVariantInsertNew NULL,[3]
	--END

	
	Insert into ED_ProductData
	Select Distinct([3]),@CustomerID,[4]+' '+[5]+' '+[6],[1],SType.SpecificationAttributeOptionID,[2],[4],
			SYear.SpecificationAttributeOptionID,
			[5],SMake.SpecificationAttributeOptionID,[6],[7],[8],[9],
			SBody.SpecificationAttributeOptionID,[10],
			--SMile.SpecificationAttributeOptionID,
			(Select SpecificationAttributeOptionID 
			From Nop_SpecificationAttributeOption Where [10] 
			between AttributeOptionFrom And AttributeOptionTo And SpecificationAttributeID=4),
			[11],
			[12],[13],[14],
			--SPrice.SpecificationAttributeOptionID,
			(Select SpecificationAttributeOptionID 
			From Nop_SpecificationAttributeOption Where [11] 
			between AttributeOptionFrom And AttributeOptionTo And SpecificationAttributeID=5),
			[15],
			[16],[17],[18],[19],
			[20],[21],[22],[23],[24],[25],SWarranty.SpecificationAttributeOptionID,[26],[27],[28],getdate(),3,'',[29],[30],[31],dbo.GetStateProvinceID([32])
	From #Temp as T 
	Left Join Nop_SpecificationAttributeOption As SMake On Smake.Name=[5] And Smake.SpecificationAttributeID=1
	Left Join Nop_SpecificationAttributeOption As SBody On SBody.Name=[9] And SBody.SpecificationAttributeID=2
	Left Join Nop_SpecificationAttributeOption As SYear On SYear.Name=[4] And SYear.SpecificationAttributeID=3
	Left Join Nop_SpecificationAttributeOption As SType On SType.Name=[1] And SType.SpecificationAttributeID=8
	Left Join Nop_SpecificationAttributeOption As SWarranty On SWarranty.Name=[25] And SWarranty.SpecificationAttributeID=9
	----Left Join Nop_SpecificationAttributeOption As SMile On [10] between SMile.AttributeOptionFrom and SMile.AttributeOptionTo
	----Left Join Nop_SpecificationAttributeOption As SPrice On [10] between SPrice.AttributeOptionFrom and SPrice.AttributeOptionTo
		
	
	select * from ED_ProductData
	
	
	set @EDProductID=SCOPE_IDENTITY()

		Update ED_ProductData
		SET
		ParaId = 1
		FROM ED_ProductData as EP
		LEFT JOIN Nop_Product as Product On Product.VIN = EP.VIN
		Where Product.Deleted = 0 And EP.CustomerID=@CustomerID
	
	
	--Update Nop_Product
	--Set
	--		[Name]=EP.VehicleName,
	--		VIN=EP.VIN,
	--		CustomerID=EP.CustomerID,
	--		VehicleName=EP.VehicleName,
	--		[Type]=EP.[Type],
	--		[TypeAttribute] =EP.[TypeAttribute],
	--		Stock=EP.Stock,
	--		[Year]=EP.[Year],
	--		YearAttribute=EP.YearAttribute,
	--		Make=EP.Make,
	--		MakeAttribute=EP.MakeAttribute,
	--		Model=EP.Model,
	--		Trim=EP.Trim,
	--		Free_Text=EP.Free_Text,
	--		Body=EP.Body,
	--		BodyAttribute=EP.BodyAttribute,
	--		Mileage=EP.Mileage,
	--		MileageAttribute=EP.MileageAttribute,
	--		Price_Current=EP.Price_Current,
	--		Reserved=EP.Reserved,
	--		Price_Wholesale=EP.Price_Wholesale,
	--		Price_Cost=EP.Price_Cost,
	--		PriceAttribute=EP.PriceAttribute,
	--		Title=EP.Title,
	--		Condition=EP.Condition,
	--		Exterior_Color=EP.Exterior_Color,
	--		Interior_Color=EP.Interior_Color,
	--		Doors=EP.Doors,
	--		Engine=EP.Engine,
	--		Transmission=EP.Transmission,
	--		Fuel_Type=EP.Fuel_Type,
	--		Drive_Type=EP.Drive_Type,
	--		Options=EP.Options,
	--		Warranty=EP.Warranty,
	--		WarrantyAttribute =EP.WarrantyAttribute,
	--		Description=EP.Description,
	--		Pics=EP.Pics,
	--		Date_in_Stock=EP.Date_in_Stock,
	--		Dealer_Name=EP.Dealer_Name,
	--		Dealer_Zip=EP.Dealer_Zip,
	--		City=EP.City,
	--		StateID=EP.StateID,
	--		[FileName]=EP.[FileName],
	--		IsNew=0,
	--		UpdatedOn = Getdate()
		
	--from Nop_Product as Product 
	--LEFT JOIN ED_ProductData as EP On EP.VIN = Product.VIN 
	--Where EP.ParaId = 1 And Product.Deleted = 0 
	
	---- for Product Picture 
		
	--If Object_Id('tempdb..#TempProductPicture') Is Not Null 
	--Drop Table #TempProductPicture

	--Create Table #TempProductPicture
	--(
	--	Id8 Bigint IDENTITY(1,1),
	--	ProductVIN varchar(MAX),
	--	ProductPictureList varchar(MAX)
	--)

	--Insert Into #TempProductPicture
	--Select Distinct([3]),[27] From #Temp 
	
	--Declare @Count8 As Bigint
	--Declare @cnt8 as bigint
	--Declare @Temp8 as varchar(MAX)
	--Declare @Temp9 as varchar(MAX)
	--Set @cnt8=0
	--Select @Count8=Max(Id8) From #TempProductPicture

	--While (Select @cnt8)<@count8
	--Begin
	--	Set @cnt8 = @cnt8 + 1
	--	Select @Temp8= ProductVIN from #TempProductPicture where Id8=@cnt8
	--	Select @Temp9= ProductPictureList from #TempProductPicture where Id8=@cnt8
	--	Set @Temp8 = LTrim(RTrim(@Temp8))
	--	Set @Temp9 = LTrim(RTrim(@Temp9))
	--	EXEC dbo.Nop_ProductPictureInsertNew NULL,@Temp8,@Temp9
	--End	
	
	--If Object_Id('tempdb..#TempProductPicture') Is Not Null 
	--Drop Table #TempProductPicture
	
	
	Delete 
	FROM ED_ProductData 
	WHERE ParaId = 1 And CustomerID=@CustomerID
	
	If Object_Id('tempdb..#Temp') Is Not Null 
	Drop Table #Temp
END