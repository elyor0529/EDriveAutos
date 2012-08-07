-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Update expert advice document path
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateExpertDocumentPath]
	@EDId bigint,
	@DocumentPath nvarchar(150)
	
AS
BEGIN
	SET NOCOUNT ON;

    Update ED_ExpertDocuments set DocumentPath=@DocumentPath Where IsActive=1
	And EDId=@EDId
END

