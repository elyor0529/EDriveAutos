-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Insert Expert Document
-- =============================================

CREATE PROCEDURE [dbo].[ED_GetAllVehicleOptions]

@ProductID  int
	
AS
BEGIN

	SET NOCOUNT OFF
	
	Declare @Options varchar(max)
	
	select @Options = p.Options from Nop_Product as p where ProductId = @ProductID
	
	print @Options
	
	If Object_Id('tempdb..#TempOptionsNew') Is Not Null 
	Drop Table #TempOptionsNew

	Create Table #TempOptionsNew
	(
		Id Bigint IDENTITY(1,1),
		VehicleOptionId Bigint,
	)  
	
	Insert Into #TempOptionsNew
	Select data as VehicleOptionId From dbo.NOP_splitstring_to_table(@Options,',')
		
	--select * from #TempOptionsNew
	
	select vo.VehicleOptionName from ED_VehicleOptions as vo
	inner join #TempOptionsNew as tn
	On vo.VehicleOptionId = tn.VehicleOptionId
	
	 order by DisplayOrder ,vo.VehicleOptionId desc
END