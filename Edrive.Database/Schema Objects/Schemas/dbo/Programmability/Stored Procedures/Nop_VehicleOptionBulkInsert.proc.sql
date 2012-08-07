--EXEC [dbo].[Nop_VehicleOptionBulkInsert]'Compact Disc Player|Air Conditioning|Power Steering|Power Brakes|Power Door Locks|Power Windows|Radial Tires|Clock|Trip Odometer|Tilt Steering Wheel|Cruise Control|Interval Wipers|Rear Defroster|Carpeting|Vanity Mirror|Day/Night Lever|Dual Sport Mirrors|Reclining Seats|Cloth Upholstery|Center Arm Rest|Power Driver`s Seat|Deluxe Wheel Covers|Inside Hood Release|Maintenance Free Battery|Courtesy Lights|Map Lights|Anti-Lock Braking System|AM/FM Stereo Radio|Drivers Side Remote Mirror|Passenger`s Front Airbag|12 Volt Power Socket|Split Front Bench Seat|Driver`s Front Airbag'
CREATE PROCEDURE [dbo].[Nop_VehicleOptionBulkInsert]

	@OptionsName VARCHAR(MAX)

AS
BEGIN

	If Object_Id('tempdb..#TempOptions') Is Not Null 
	Drop Table #TempOptions

	Create Table #TempOptions
	(
		Id Bigint IDENTITY(1,1),
		OptionName varchar(MAX),
		OptionId int NULL
	)    
	
	Declare @Options varchar(max)
	Select @Options = replace(@OptionsName,'''','`')

	print('Options' +@Options)
	
	Insert Into #TempOptions
	Select data as OptionName,Null From dbo.NOP_splitstring_to_table(@Options,'|')
		
	--select * from #TempOptions
	
	Insert Into [ED_VehicleOptions] (VehicleOptionName,DisplayOrder)
	Select RTrim(LTrim(OptionName)),1 from #TempOptions 
	Where RTrim(LTRIM(OptionName)) Not In
	(Select VehicleOptionName from ED_VehicleOptions)
	
	If Object_Id('tempdb..#TempOptions') Is Not Null 
	Drop Table #TempOptions
END