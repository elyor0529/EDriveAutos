-- =============================================
-- Author:		<Manali>
-- Create date: <29/3/2011>
-- Description:	<Get Products>
-- =============================================

--EXEC [dbo].[ED_GetAllAttributeOfProduct] 1
CREATE PROCEDURE [dbo].[ED_GetAllAttributeOfProduct]
	
	@tagId int
AS
BEGIN
	SET NOCOUNT ON;
	
	
	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where SettingID=376
	
	If(@tagId = 1)
	Begin
	
	select distinct p.Mileage from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Mileage
	
	End
	
	Else IF(@tagId = 2)
	Begin
	
	select distinct p.Transmission from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Transmission
		
	End
	
	Else IF(@tagId = 3)
	Begin
	
	select distinct p.Trim from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Trim
	End
	
	Else If(@tagId = 4)
	Begin
	
	select distinct p.Engine from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Engine
	End
	
	Else If(@tagId = 5)
	Begin

	select distinct p.Interior_Color from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Interior_Color
	End
	
		Else If(@tagId = 6)
	Begin

	select distinct p.Exterior_Color from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Exterior_Color
	End
	
		Else If(@tagId = 7)
	Begin

	select distinct p.Drive_Type from nop_product as p
	where p.Deleted=0 and p.Published = 1 And 
	DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0 And p.QualifyPrice is not null
	order by p.Drive_Type
	End
	
    
END