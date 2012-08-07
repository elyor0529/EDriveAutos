-- =============================================
-- Author:		<Manali>
-- Create date: <4/2/2011>
-- Description:	<Get testimonials>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetTestimonials]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM ED_Testimonials 
	WHERE IsActive=1
	ORDER BY createdOn desc
END

