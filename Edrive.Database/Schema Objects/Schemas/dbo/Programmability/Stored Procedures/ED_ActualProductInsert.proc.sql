-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <12/3/2011>
-- Description:	Insert Product And Product Variant from Product Data
-- =============================================
CREATE PROCEDURE [dbo].[ED_ActualProductInsert] --'19XFA1F85BE016991,',38
	@VIN varchar(MAX),
	@CustomerID int,
	@PrdID bigint output
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int
	DECLARE @ParaId int
	DECLARE @Count As Bigint
	DECLARE @cnt as bigint
	DECLARE @Temp as varchar(MAX)
	DECLARE @ProductID int
	
	If Object_Id('tempdb..#TempVIN') Is Not Null 
	Drop Table #TempVIN

	Create Table #TempVIN
	(
		Id Bigint IDENTITY(1,1),
		VINumber varchar(MAX)
	)

	Insert Into #TempVIN
	Select * From dbo.NOP_splitstring_to_table(@VIN,',')
	
	--Select * From #TempVIN

	
	
	Set @cnt=0
	Select @Count=Max(Id) From #TempVIN

	While (Select @cnt)<@count
	Begin
		Set @cnt = @cnt + 1
		Select @Temp= VINumber from #TempVIN where Id=@Cnt
		
		EXEC [dbo].[ED_ProductAndVariantInsert] @Temp,@PrdID output
		EXEC [dbo].[ED_DeleteProductDataByVIN] @Temp,@CustomerID
	End
	
END