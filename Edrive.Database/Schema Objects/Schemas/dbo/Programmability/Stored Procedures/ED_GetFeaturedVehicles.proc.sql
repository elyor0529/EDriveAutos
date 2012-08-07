-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <31/3/2011>
-- Description:	Get 10 Featured Vehicles
-- =============================================

CREATE PROCEDURE [dbo].[ED_GetFeaturedVehicles]

AS
BEGIN

	Declare @iDays as int
	Select @iDays = [Value] FROM Nop_Setting where Name='CountDown.Days'

	SELECT top(4)
		p.[ProductId],
		p.[Name],
		p.[CreatedOn], 
		p.[VIN],
		p.[Mileage],
		p.[Price_Current]
	FROM Nop_Product p
	WHERE 
		p.Published = 1 AND p.Deleted=0 and p.IsFeature = 1
		and p.[QualifyPrice] Is Not Null and (DATEDIFF(s,getdate(),DATEADD(d,@iDays,p.UpdatedOn)) > 0) 
	ORDER BY
		p.CreatedOn	DESC
END