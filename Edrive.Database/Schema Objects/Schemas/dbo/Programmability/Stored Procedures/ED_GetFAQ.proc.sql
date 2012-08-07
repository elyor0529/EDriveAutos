-- =============================================
-- Author:		<Manali>
-- Create date: <3/2/2011>
-- Description:	Get FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetFAQ]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM ED_Faq 
	WHERE IsActive=1
	ORDER BY OrderId
END



