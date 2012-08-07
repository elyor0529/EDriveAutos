-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Delete Products
-- =============================================
CREATE PROCEDURE [dbo].[ED_getEdriveManagement] 
	
AS
BEGIN
	SET NOCOUNT ON;
	
	Select  * from 
	ED_EManagement 
	
	WHERE Deleted=0 and Published = 1
	order by DisplayOrder asc


END