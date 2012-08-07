-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Update picture of product
-- =============================================
Create PROCEDURE [dbo].[ED_UpdateEdriveManagementPicture]
	@ManagementId int,
	@ImageID int
	
AS
BEGIN
	SET NOCOUNT ON;

    Update ED_EManagement set ImageID=@ImageID Where Published=1
	And ManagementId=@ManagementId
END