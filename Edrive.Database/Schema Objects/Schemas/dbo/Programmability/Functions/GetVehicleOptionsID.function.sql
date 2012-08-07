--SELECT * FROM dbo.GetVehicleOptionsID('Compact Disc Player|Air Conditioning|Side Impact Airbag(s)|Power Steering|Power Brakes|Power Door Locks|Power Windows|Radial Tires|Alloy Wheels|Gauge Cluster|Clock|Trip Odometer|Tachometer|Tilt Steering Wheel|Cruise Control|Interval Wipers|Rear Defroster|Carpeting|Vanity Mirror|Dual Sport Mirrors|Front Bucket Seats|Reclining Seats|Center Arm Rest|Power Driver`s Seat|Power Passenger Seat|Climate Control|Inside Hood Release|Maintenance Free Battery|Courtesy Lights|Leather Upholstery|Rear Window Wiper|Map Lights|Anti-Lock Braking System|AM/FM Stereo Radio|Center Console|Drivers Side Remote Mirror|4 Wheel Disc Brakes|Passenger`s Front Airbag|12 Volt Power Socket|Driver`s Front Airbag')

CREATE FUNCTION [dbo].[GetVehicleOptionsID] 
(
    @string VARCHAR(MAX)
)
RETURNS @output TABLE(
        --Id Bigint IDENTITY(1,1),
		OptionName varchar(MAX)
		--OptionId int NULL
)
BEGIN
    
       DECLARE @MyTable       
        TABLE(
        Id Bigint IDENTITY(1,1),
		OptionName varchar(MAX),
		OptionId int NULL)
		
                
        INSERT INTO @MyTable  
		Select data as OptionName,OptionId=b.VehicleOptionId From dbo.NOP_splitstring_to_table(@string,'|')
		left join ED_VehicleOptions as B On data = b.VehicleOptionName
		
		
		
		Declare @TempStr as varchar(MAX)
		SELECT @TempStr = coalesce(@TempStr+',','') + CONVERT(varchar, OptionId)
		FROM @MyTable
		
	
		
		INSERT INTO @output(OptionName) values (@TempStr) 
			
		
    RETURN
END