-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Delete Products
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteEdriveManagement] 
	@ManagementId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_EManagement SET
	Deleted=1
	WHERE ManagementId=@ManagementId


END