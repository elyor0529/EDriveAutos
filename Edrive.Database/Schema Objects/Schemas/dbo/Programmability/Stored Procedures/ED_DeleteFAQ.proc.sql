-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Delete FAQ
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteFAQ] --'134'
	@FaqId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_Faq SET
	IsActive=0
	WHERE FaqId=@FaqId


END

