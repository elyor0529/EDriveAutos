-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Delete Dealers FAQ
-- =============================================
Create PROCEDURE [dbo].[ED_DealersDeleteFAQ] --'134'
	@FaqId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_DealersFaq SET
	IsActive=0
	WHERE FaqId=@FaqId


END