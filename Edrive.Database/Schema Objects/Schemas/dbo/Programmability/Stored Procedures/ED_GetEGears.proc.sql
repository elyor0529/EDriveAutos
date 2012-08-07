-- =============================================
-- Author:		<Manali>
-- Create date: <29/3/2011>
-- Description:	<Get Products>
-- =============================================
Create PROCEDURE [dbo].[ED_GetEGears]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM ED_EGear 
	WHERE Deleted=0
	ORDER BY DisplayOrder asc
END