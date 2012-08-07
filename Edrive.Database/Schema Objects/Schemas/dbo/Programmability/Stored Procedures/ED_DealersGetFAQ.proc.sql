-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get Dealers FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_DealersGetFAQ]
	
AS
BEGIN
	SET NOCOUNT ON;
    SELECT *
	FROM ED_DealersFaq
	WHERE IsActive=1
	ORDER BY OrderId
END