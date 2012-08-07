-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <14/3/2011>
-- Description:	Update Product Specification Attribute Option
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateProductSpecByAttribute] --'33585700','0,0,0'
	@VIN varchar(MAX),
	@Attribute varchar(MAX)
		
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @Count As Bigint
	DECLARE @cnt as bigint
	DECLARE @Temp as int
	
	If Object_Id('tempdb..#TempAttr') Is Not Null 
	Drop Table #TempAttr

	Create Table #TempAttr
	(
		Id Bigint IDENTITY(1,1),
		Attr int
	)

	Insert Into #TempAttr
	Select * From dbo.NOP_splitstring_to_table(@Attribute,',')
	--Select * From #TempAttr

	Set @cnt=0
	Select @Count=Max(Id) From #TempAttr

	While (Select @cnt)<@count
	Begin
		Set @cnt = @cnt + 1
		Select @Temp = Attr from #TempAttr where Id=@cnt
	
		Declare @bltrue bit	
		Set @bltrue = 'true'
		if(@Temp = 0)
		Begin
			Set @bltrue = 'false'
			Set @Temp = 1
		End

		Update Nop_Product_SpecificationAttribute_Mapping
		Set SpecificationAttributeOptionID=@Temp,
			IsActive=@bltrue 
		From Nop_Product_SpecificationAttribute_Mapping As PSM
		Left Join Nop_Product As P on PSM.ProductID=P.ProductID
		Where P.VIN=@VIN And PSM.SpecificationAttributeID=@cnt
	End

END





