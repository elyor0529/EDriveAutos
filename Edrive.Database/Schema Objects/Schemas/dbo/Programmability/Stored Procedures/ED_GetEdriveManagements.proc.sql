-- =============================================
-- Author:		<Manali>
-- Create date: <29/3/2011>
-- Description:	<Get Products>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetEdriveManagements]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM ED_EManagement 
	WHERE Deleted=0
	ORDER BY DisplayOrder asc
END