-- =============================================
-- Author:		<Manali>
-- Create date: <3/2/2011>
-- Description:	Get Expert Documents
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetExpertDocuments]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM ED_ExpertDocuments 
	WHERE IsActive=1
	ORDER BY CreatedOn
END





