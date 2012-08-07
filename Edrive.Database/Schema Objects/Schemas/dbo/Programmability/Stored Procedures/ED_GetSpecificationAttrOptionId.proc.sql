-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <11/3/2011>
-- Description:	Get Specification Attribute Option Id
-- Parameter @Options: Pass comma seperated Specification Attribute like
--						'Make,Body,Year,Mileage,Price'
-- If Attribute is blank then pass ''
-- Result Row wise
-- 1st Row : Make
-- 2nd Row : Body
-- 3rd Row : Year
-- 4th Row : Mileage
-- 5th Row : Price
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetSpecificationAttrOptionId] --'Mercedes,Sedan,2011,17424,18000.00'
(
	@Options varchar(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	Declare @Count As Bigint
	Declare @cnt as bigint
	Declare @Temp as varchar(MAX)
	Declare @TempNA As Bigint
		
	Set @TempNA = 0

	If Object_Id('tempdb..#Spec') Is Not Null 
	Drop Table #Spec

	Create Table #Spec
	(
		Id Bigint IDENTITY(1,1),
		List varchar(MAX)
	)

	If Object_Id('tempdb..#FullList') Is Not Null 
	Drop Table #FullList

	Create Table #FullList
	(
		IdL Bigint IDENTITY(1,1),
		FList int
	)
		
	Insert Into #Spec
	Select * From dbo.NOP_splitstring_to_table(@Options,',')
	--Select * From #Spec

	Set @cnt=0
	Select @Count=Max(Id) From #Spec

	While (Select @cnt)<@count
	Begin
		Set @cnt = @cnt + 1
		Select @Temp= List from #Spec where Id=@cnt
		Set @Temp = LTrim(RTrim(@Temp))
		
		if (@Temp != '' And @Temp != '0')
		Begin
			Declare @SpecAttrOptId as int
			if (@cnt = 4 Or @cnt = 5) --For Mileage and Price Range
			Begin
				Declare @dcTemp as decimal
				Declare @intTemp as int
				Set @dcTemp = Convert(decimal(18,2),@Temp)
				Set @intTemp = Convert(int,@dcTemp)
				Set @SpecAttrOptId = (Select SpecificationAttributeOptionID 
						From Nop_SpecificationAttributeOption 
						Where @intTemp Between AttributeOptionFrom And AttributeOptionTo
						And SpecificationAttributeID=@cnt)				
			End
			Else
			Begin
				Set @SpecAttrOptId = (Select SpecificationAttributeOptionID 
						From Nop_SpecificationAttributeOption
						Where Name=@Temp And SpecificationAttributeID=@cnt and DisplayOrder = 1)	
				
				If (@SpecAttrOptId Is null)
				Begin
					EXEC [dbo].[Nop_SpecificationAttributeOptionInsert] null,@cnt,@Temp,1,0,0
					Set @SpecAttrOptId = (Select SpecificationAttributeOptionID 
						From Nop_SpecificationAttributeOption
						Where Name=@Temp And SpecificationAttributeID=@cnt)	
				End				
			End			
			Insert Into #FullList
			Select IsNull(@SpecAttrOptId,0)
		End
		Else
		Begin
			--If Attribute is blank('') then return 0
			Insert Into #FullList
			Select @TempNA
		End
	End
	
	Select FList From #FullList
	
	DROP TABLE #Spec
	DROP TABLE #FullList
	SET @Err = @@Error

	RETURN @Err
END




