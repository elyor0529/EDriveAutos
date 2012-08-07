-- =============================================
-- Author:		<Manali>
-- Create date: <29/3/2011>
-- Description:	<Get Products>
-- =============================================

--EXEC [dbo].[ED_GetAllAttributeOfProductNew]
CREATE PROCEDURE [dbo].[ED_GetAllAttributeOfProductNew]
	
AS
BEGIN
	SET NOCOUNT ON;
	
	
	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where SettingID=376
	
	
	
	--select distinct p.Mileage as FilterName ,'Mileage' as FilterType from nop_product as p
	--where p.Published = 1 And p.Deleted=0 and 
	--DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	
	--union
	
		select distinct p.Transmission as FilterName ,'Transmission' as FilterType  from nop_product as p
	where p.Published = 1 And p.Deleted=0 and 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	--union
	
	--select distinct p.Trim as FilterName , 'Trim' as FilterType  from nop_product as p
	--where p.Published = 1 And p.Deleted=0 and 
	--DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	
	union
	
	select distinct p.Engine as FilterName, 'Engine' as FilterType  from nop_product as p
	where p.Published = 1 And p.Deleted=0 and 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	
	--union 
	
	--select distinct p.Interior_Color as FilterName,'IColor' as FilterType  from nop_product as p
	--where p.Published = 1 And p.Deleted=0 and 
	--DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	
	--	union 
	
	--select distinct p.Exterior_Color as FilterName ,'EColor' as FilterType  from nop_product as p
	--where p.Published = 1 And p.Deleted=0 and 
	--DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	
	union
	
	select distinct p.Drive_Type as FilterName,'DriveType' as FilterType  from nop_product as p
	where p.Published = 1 And p.Deleted=0 and 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
    
END