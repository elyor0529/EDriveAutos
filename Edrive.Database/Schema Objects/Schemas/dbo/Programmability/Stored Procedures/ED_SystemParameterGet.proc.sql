-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get Dealers FAQ
-- =============================================
Create PROCEDURE [dbo].[ED_SystemParameterGet]
	
AS
BEGIN
	SET NOCOUNT ON;
    SELECT *
	FROM ED_SystemParameter
	WHERE IsActive=1
	ORDER BY ParameterID
END