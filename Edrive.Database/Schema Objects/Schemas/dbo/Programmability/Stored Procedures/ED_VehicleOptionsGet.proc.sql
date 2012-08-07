-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get Dealers FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_VehicleOptionsGet]
	
AS
BEGIN
	SET NOCOUNT ON;
    SELECT *
	FROM ED_VehicleOptions
	ORDER BY VehicleOptionName asc
END