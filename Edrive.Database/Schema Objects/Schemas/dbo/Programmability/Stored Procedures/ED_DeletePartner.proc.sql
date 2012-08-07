-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Delete Partner
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeletePartner] --'134'
	@PartnerId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_Partners SET
	IsActive=0
	WHERE PartnerId=@PartnerId


END


