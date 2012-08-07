-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Delete Testimonials
-- =============================================
CREATE PROCEDURE [dbo].[ED_DeleteTestimonial] 
	@TId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ED_Testimonials SET
	IsActive=0
	WHERE TId=@TId


END
