-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--declare @intResult nvarchar
--exec ED_GetVehicleOptionIDBYName 'Aluminum/Alloy Wheels|Boston Acoustics Stereo|Leather Seats|Navigation System|Power Sunroof|Theft Recovery Sys|W/out Power Seat'
CREATE PROCEDURE [dbo].[ED_GetVehicleOptionIDBYName] 
	-- Add the parameters for the stored procedure here
	@vehicleOptions nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    If Object_Id('tempdb..#TempOptions') Is Not Null 
	Drop Table #TempOptions

	Create Table #TempOptions
	(
		Id Bigint IDENTITY(1,1),
		OptionName varchar(MAX),
		OptionId int NULL
	)    
	
	Insert Into #TempOptions
	Select data as OptionName,Null From dbo.NOP_splitstring_to_table(@vehicleOptions,'|')
	
	--select * from #TempOptions 
	
	Insert Into [ED_VehicleOptions] (VehicleOptionName,DisplayOrder)
	Select RTrim(LTrim(OptionName)),1 from #TempOptions 
	Where RTrim(LTRIM(OptionName)) Not In
	(Select VehicleOptionName from ED_VehicleOptions)
    
    update #TempOptions set OptionId = B.VehicleOptionId  from #TempOptions 
    left join ED_VehicleOptions as B On OptionName = b.VehicleOptionName
       
    --select * from #TempOptions
   
	
	Declare @vehicleOptionsID As nvarchar(Max) 
	SELECT @vehicleOptionsID = coalesce(@vehicleOptionsID+',','') + CONVERT(nvarchar, OptionId)
	FROM #TempOptions
	
	select @vehicleOptionsID as Options
	
	--Print (@vehicleOptionsID)
	
END