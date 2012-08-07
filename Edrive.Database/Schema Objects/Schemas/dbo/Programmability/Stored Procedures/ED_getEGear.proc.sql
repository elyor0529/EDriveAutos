-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Delete Products
-- =============================================
Create PROCEDURE [dbo].[ED_getEGear] 
	
AS
BEGIN
	SET NOCOUNT ON;
	
	Select  * from 
	ED_EGear 
	
	WHERE Deleted=0 and Published = 1
	order by DisplayOrder asc


END