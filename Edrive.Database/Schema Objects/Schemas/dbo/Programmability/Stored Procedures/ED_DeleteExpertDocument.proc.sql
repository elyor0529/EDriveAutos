-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Delete Expert Document
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteExpertDocument] 
	@EDId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_ExpertDocuments SET
	IsActive=0
	WHERE EDId=@EDId


END


