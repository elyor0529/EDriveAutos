-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Delete Products
-- =============================================
Create PROCEDURE [dbo].[ED_DeleteEGear] 
	@eGearID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_EGear SET
	Deleted=1
	WHERE eGearID = @eGearID


END