-- =============================================
-- Author:		<Manali>
-- Create date: <28/3/2011>
-- Description:	<Get testimonial for Home page>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetTop1Testimonial]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Top 1 TId,TContent,CreatedOn
	FROM ED_Testimonials 
	WHERE IsActive=1
	ORDER BY createdOn desc
END


