-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <16/3/2011>
-- Description:	Insert Product Specification Mapping
-- =============================================

--exec [dbo].[ED_ProductSpecMappingInsert] 20
CREATE PROCEDURE [dbo].[ED_ProductSpecMappingInsert] --62
	@ProductID int
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int
	DECLARE @ParaId int
	DECLARE @Count As Bigint
	DECLARE @cnt as bigint
	DECLARE @Temp as int
			
	If Object_Id('tempdb..#TempSpec') Is Not Null 
	Drop Table #TempSpec

	Create Table #TempSpec
	(
		Id Bigint IDENTITY(1,1),
		SpecAttrID int
	)

	Insert Into #TempSpec
	Select SpecificationAttributeID From Nop_SpecificationAttribute where DisplayOrder != 0
	--Select * From #TempSpec

	Set @cnt=0
	Select @Count=Max(Id) From #TempSpec

	While (Select @cnt)<@count
	Begin
		Set @cnt = @cnt + 1
		Select @Temp= SpecAttrID from #TempSpec where Id=@Cnt
	
		Declare @intSpecAttrOptId as int
		Declare @blActive as bit
		Set @intSpecAttrOptId = 1	
		Set @blActive = 1
		
		If (@Temp = 1) --Make
		Begin
			Set @intSpecAttrOptId = (Select MakeAttribute From Nop_Product Where ProductID=@ProductID)
		End
		Else If (@Temp = 2) --Body
		Begin
			Set @intSpecAttrOptId = (Select BodyAttribute From Nop_Product Where ProductID=@ProductID)
		End
		Else If (@Temp = 3) --Year
		Begin
			Set @intSpecAttrOptId = (Select YearAttribute From Nop_Product Where ProductID=@ProductID)
		End
		Else If (@Temp = 4) --Mileage
		Begin
			Set @intSpecAttrOptId = (Select MileageAttribute From Nop_Product Where ProductID=@ProductID)
		End
		Else If (@Temp = 5) --Price
		Begin
			Set @intSpecAttrOptId = (Select PriceAttribute From Nop_Product Where ProductID=@ProductID)
		End
		Else If (@Temp = 8) --Price
		Begin
			Set @intSpecAttrOptId = (Select TypeAttribute From Nop_Product Where ProductID=@ProductID)
		End
		Else If (@Temp = 9) --Price
		Begin
			Set @intSpecAttrOptId = (Select WarrantyAttribute From Nop_Product Where ProductID=@ProductID)
		End
		
		if (@intSpecAttrOptId<>0)
		Begin
		
			If (@intSpecAttrOptId = 0)
				Set @intSpecAttrOptId = 1
			If (@intSpecAttrOptId = 1)
				Set @blActive = 0

			EXEC [dbo].[Nop_Product_SpecificationAttribute_MappingInsert] null,@ProductID,@intSpecAttrOptId,1,1,1,@Temp,@blActive
		End			
	End
	
END