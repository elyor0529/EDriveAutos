-- =============================================
-- Author:		<Manali>
-- Create date: <28/3/2011>
-- Description:	<Get Partners>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetPartnersHome]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT PartnerId,CreatedOn
			,PictureId
	FROM ED_Partners 
	WHERE IsActive=1 And IsApproved=1
	ORDER BY CreatedOn desc
END




