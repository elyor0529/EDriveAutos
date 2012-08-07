-- =============================================
-- Author:		<Manali>
-- Create date: <28/3/2011>
-- Description:	<Get Partners>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetPartners]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT PartnerId,FirstName + ' ' + LastName As Name,Company,Email,CreatedOn
			,PictureId
	FROM ED_Partners 
	WHERE IsActive=1
	ORDER BY CreatedOn desc
END



