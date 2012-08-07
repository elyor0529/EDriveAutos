-- =============================================
-- Author:		<Manali>
-- Create date: <29/3/2011>
-- Description:	<Get Products>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetEdriveProducts]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM ED_EdriveProducts 
	WHERE IsActive=1
	ORDER BY DisplayOrder asc
END